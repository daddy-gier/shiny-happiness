#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class NH_SceneAuditTool : EditorWindow
{
    [MenuItem("Nyghtshade/Run Full Audit")]
    public static void RunFullAudit()
    {
        var window = GetWindow<NH_SceneAuditTool>("Scene Audit");
        window.Show();
        window.RunAudit();
    }

    private string _report = "";
    private Vector2 _scroll;

    void RunAudit()
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== NYGHTSHADE SCENE AUDIT ===");
        sb.AppendLine($"Time: {System.DateTime.Now}\n");

        var allGOs = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        sb.AppendLine($"Total GameObjects: {allGOs.Length}");

        int active = 0, meshRenderers = 0, meshColliders = 0, lights = 0, realtimeLights = 0, cameras = 0, animators = 0;
        int missingScripts = 0, audioListeners = 0, mainCameras = 0;
        var nameCounts = new Dictionary<string, int>();

        foreach (var go in allGOs)
        {
            if (go.activeInHierarchy) active++;

            if (!nameCounts.ContainsKey(go.name)) nameCounts[go.name] = 0;
            nameCounts[go.name]++;

            var components = go.GetComponents<Component>();
            foreach (var c in components)
                if (c == null) missingScripts++;

            if (go.GetComponent<MeshRenderer>() != null) meshRenderers++;
            if (go.GetComponent<MeshCollider>() != null) meshColliders++;
            if (go.GetComponent<Animator>() != null) animators++;

            var al = go.GetComponent<AudioListener>();
            if (al != null) audioListeners++;

            var cam = go.GetComponent<Camera>();
            if (cam != null) { cameras++; if (go.CompareTag("MainCamera")) mainCameras++; }

            var light = go.GetComponent<Light>();
            if (light != null)
            {
                lights++;
                if (light.lightmapBakeType == LightmapBakeType.Realtime) realtimeLights++;
            }
        }

        sb.AppendLine($"Active GOs: {active}");
        sb.AppendLine($"MeshRenderers: {meshRenderers}");
        sb.AppendLine($"MeshColliders: {meshColliders}");
        sb.AppendLine($"Animators: {animators}");
        sb.AppendLine($"Cameras: {cameras} (Main: {mainCameras})");
        sb.AppendLine($"Lights: {lights} (Realtime: {realtimeLights})");
        sb.AppendLine($"AudioListeners: {audioListeners}");
        sb.AppendLine($"Missing Scripts: {missingScripts}");

        sb.AppendLine("\n--- WARNINGS ---");
        if (mainCameras > 1) sb.AppendLine($"WARNING: {mainCameras} Main Cameras detected!");
        if (audioListeners > 1) sb.AppendLine($"WARNING: {audioListeners} AudioListeners detected!");
        if (missingScripts > 0) sb.AppendLine($"WARNING: {missingScripts} missing script components!");
        if (meshColliders > 1000) sb.AppendLine($"WARNING: {meshColliders} MeshColliders - HIGH COST. Target < 300.");
        if (realtimeLights > 100) sb.AppendLine($"WARNING: {realtimeLights} realtime lights - HIGH COST.");

        sb.AppendLine("\n--- TOP 10 OBJECT NAMES ---");
        var sorted = new List<KeyValuePair<string, int>>(nameCounts);
        sorted.Sort((a, b) => b.Value.CompareTo(a.Value));
        for (int i = 0; i < Mathf.Min(10, sorted.Count); i++)
            sb.AppendLine($"  {sorted[i].Key}: {sorted[i].Value}");

        _report = sb.ToString();
        Debug.Log(_report);
    }

    void OnGUI()
    {
        if (GUILayout.Button("Run Audit")) RunAudit();
        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        EditorGUILayout.TextArea(_report, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
    }
}
#endif
