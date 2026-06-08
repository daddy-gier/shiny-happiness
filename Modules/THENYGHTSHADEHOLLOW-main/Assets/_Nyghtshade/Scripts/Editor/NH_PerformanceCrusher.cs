#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class NH_PerformanceCrusher : EditorWindow
{
    [MenuItem("Nyghtshade/Crush Lag Now")]
    public static void CrushLag()
    {
        string reportPath = "Assets/NYGHTSHADE_PERFORMANCE_CRUSH_REPORT.md";

        var beforeObjects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var beforeLights = Object.FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var beforeMeshColliders = Object.FindObjectsByType<MeshCollider>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        int beforeGO = beforeObjects.Length;
        int beforeRealtimeLights = beforeLights.Count(l => l != null && l.enabled && l.lightmapBakeType == LightmapBakeType.Realtime);
        int beforeShadowLights = beforeLights.Count(l => l != null && l.enabled && l.shadows != LightShadows.None);
        int beforeMeshColliderCount = beforeMeshColliders.Length;

        GameObject disabledLightsRoot = GameObject.Find("_DISABLED_DUPLICATE_LIGHTS");
        if (disabledLightsRoot == null) disabledLightsRoot = new GameObject("_DISABLED_DUPLICATE_LIGHTS");

        GameObject optimizedColliderRoot = GameObject.Find("_OPTIMIZED_COLLIDERS");
        if (optimizedColliderRoot == null) optimizedColliderRoot = new GameObject("_OPTIMIZED_COLLIDERS");

        // Enable GPU Instancing on all project materials
        int matCount = 0;
        string[] matGuids = AssetDatabase.FindAssets("t:Material");
        foreach (string guid in matGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat != null && !mat.enableInstancing)
            {
                Undo.RecordObject(mat, "Enable GPU Instancing");
                mat.enableInstancing = true;
                EditorUtility.SetDirty(mat);
                matCount++;
            }
        }

        // Optimize lights — keep one directional, remove shadows from point lights, bake decorative
        int lightsOptimized = 0;
        int lightsDisabled = 0;
        bool keptMainDirectional = false;

        var lights = Object.FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var light in lights)
        {
            if (light == null) continue;
            Undo.RecordObject(light, "Optimize Lights");

            if (light.type == LightType.Directional)
            {
                if (!keptMainDirectional && light.enabled)
                {
                    keptMainDirectional = true;
                    light.shadows = LightShadows.Soft;
                    light.lightmapBakeType = LightmapBakeType.Mixed;
                }
                else
                {
                    light.shadows = LightShadows.None;
                    light.lightmapBakeType = LightmapBakeType.Baked;
                    light.enabled = false;
                    light.transform.SetParent(disabledLightsRoot.transform);
                    lightsDisabled++;
                }
            }
            else
            {
                light.shadows = LightShadows.None;
                lightsOptimized++;

                if (light.type == LightType.Point && light.intensity <= 1.5f)
                {
                    light.enabled = false;
                    light.transform.SetParent(disabledLightsRoot.transform);
                    lightsDisabled++;
                }
                else
                {
                    light.lightmapBakeType = LightmapBakeType.Baked;
                }
            }

            EditorUtility.SetDirty(light);
        }

        // Remove MeshColliders from repeated static visual objects only
        // Key safety rule: only replace large objects (>2m on each axis) with BoxColliders
        // Small tiles/props/decor get their MeshColliders removed without replacement
        string[] repeatedStaticHints =
        {
            "FloorTile", "PrisonFloorTile", "BathRoomFloorTile", "TowerFloor",
            "InsideWall", "WallTile", "Ceiling", "Trim",
            "Shelf", "Rack", "Pallet", "Box", "Forklift",
            "Decor", "Prop", "Light"
        };

        int meshCollidersRemoved = 0;
        int boxCollidersAdded = 0;

        var allObjects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var go in allObjects)
        {
            if (go == null) continue;

            bool nameLooksRepeatedStatic = repeatedStaticHints.Any(h =>
                go.name.IndexOf(h, System.StringComparison.OrdinalIgnoreCase) >= 0);
            bool safeStaticVisual = go.isStatic || nameLooksRepeatedStatic;

            if (!safeStaticVisual) continue;

            var meshColliders = go.GetComponents<MeshCollider>();
            if (meshColliders == null || meshColliders.Length == 0) continue;

            Renderer renderer = go.GetComponent<Renderer>();
            Bounds bounds = renderer != null ? renderer.bounds : new Bounds(go.transform.position, Vector3.zero);

            foreach (var mc in meshColliders)
            {
                if (mc == null) continue;
                Undo.DestroyObjectImmediate(mc);
                meshCollidersRemoved++;
            }

            string nameLower = go.name.ToLower();
            bool isDecorOrProp = nameLower.Contains("decor") || nameLower.Contains("light") || nameLower.Contains("prop");
            float volume = bounds.size.x * bounds.size.y * bounds.size.z;
            bool largeEnoughForSimpleCollider =
                bounds.size.x > 2.0f && bounds.size.z > 2.0f && volume > 2.0f && !isDecorOrProp;

            if (largeEnoughForSimpleCollider && go.GetComponent<BoxCollider>() == null)
            {
                Undo.RecordObject(go, "Add Optimized BoxCollider");
                BoxCollider box = go.AddComponent<BoxCollider>();
                box.center = go.transform.InverseTransformPoint(bounds.center);
                Vector3 worldSize = bounds.size;
                box.size = new Vector3(
                    worldSize.x / Mathf.Max(0.0001f, Mathf.Abs(go.transform.lossyScale.x)),
                    worldSize.y / Mathf.Max(0.0001f, Mathf.Abs(go.transform.lossyScale.y)),
                    worldSize.z / Mathf.Max(0.0001f, Mathf.Abs(go.transform.lossyScale.z))
                );
                boxCollidersAdded++;
                EditorUtility.SetDirty(go);
            }
        }

        // Disable duplicate Main Cameras
        int duplicateMainCameras = 0;
        var cameras = Object.FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        var activeMainCameras = cameras.Where(c => c != null && c.enabled && c.CompareTag("MainCamera")).ToList();
        duplicateMainCameras = Mathf.Max(0, activeMainCameras.Count - 1);
        bool keptCamera = false;
        foreach (var cam in activeMainCameras)
        {
            if (!keptCamera) { keptCamera = true; continue; }
            Undo.RecordObject(cam, "Disable Duplicate Main Camera");
            cam.enabled = false;
            EditorUtility.SetDirty(cam);
        }

        // Disable duplicate AudioListeners
        int duplicateAudioListeners = 0;
        var listeners = Object.FindObjectsByType<AudioListener>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        var activeListeners = listeners.Where(l => l != null && l.enabled).ToList();
        duplicateAudioListeners = Mathf.Max(0, activeListeners.Count - 1);
        bool keptListener = false;
        foreach (var listener in activeListeners)
        {
            if (!keptListener) { keptListener = true; continue; }
            Undo.RecordObject(listener, "Disable Duplicate AudioListener");
            listener.enabled = false;
            EditorUtility.SetDirty(listener);
        }

        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        var afterObjects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var afterLights = Object.FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var afterMeshColliders = Object.FindObjectsByType<MeshCollider>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        int afterRealtimeLights = afterLights.Count(l => l != null && l.enabled && l.lightmapBakeType == LightmapBakeType.Realtime);
        int afterShadowLights = afterLights.Count(l => l != null && l.enabled && l.shadows != LightShadows.None);

        string report =
$@"# NYGHTSHADE PERFORMANCE CRUSH REPORT

## Before
- GameObjects: {beforeGO}
- MeshColliders: {beforeMeshColliderCount}
- Active Realtime Lights: {beforeRealtimeLights}
- Active Shadow Lights: {beforeShadowLights}

## After
- GameObjects: {afterObjects.Length}
- MeshColliders: {afterMeshColliders.Length}
- Active Realtime Lights: {afterRealtimeLights}
- Active Shadow Lights: {afterShadowLights}

## Changes Made
- Materials enabled for GPU Instancing: {matCount}
- MeshColliders removed: {meshCollidersRemoved}
- BoxColliders added (large floors/walls only): {boxCollidersAdded}
- Lights shadow-stripped: {lightsOptimized}
- Lights disabled/moved: {lightsDisabled}
- Duplicate Main Cameras disabled: {duplicateMainCameras}
- Duplicate AudioListeners disabled: {duplicateAudioListeners}

## Required Next Check
Run Play Mode and confirm:
- Player spawns
- Camera follows
- Walking collision works (no fall-through in playable areas)
- No duplicate cameras or audio listeners
- No major console errors

## Next Steps
1. Bake lighting: Window > Rendering > Lighting > Generate Lighting
2. Bake Occlusion Culling: Window > Rendering > Occlusion Culling > Bake
3. Run NH_SceneAuditTool to verify counts
4. Test Play Mode for frame rate improvement
";

        File.WriteAllText(reportPath, report);
        AssetDatabase.Refresh();

        Debug.Log($"[NYGHTSHADE] Performance crush complete. Removed {meshCollidersRemoved} MeshColliders, optimized {lightsOptimized + lightsDisabled} lights, enabled instancing on {matCount} materials. Report: {reportPath}");
    }
}
#endif
