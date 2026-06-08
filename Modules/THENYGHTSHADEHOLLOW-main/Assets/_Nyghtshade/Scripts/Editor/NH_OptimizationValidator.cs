#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Text;

public class NH_OptimizationValidator : EditorWindow
{
    [MenuItem("Nyghtshade/Validate Playable Scene")]
    public static void Validate()
    {
        var window = GetWindow<NH_OptimizationValidator>("Optimization");
        window.Show();
        window.RunValidation();
    }

    private string _report = "";
    private Vector2 _scroll;

    void RunValidation()
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== OPTIMIZATION VALIDATION ===\n");

        int meshColliders = 0, realtimeLights = 0, shadowLights = 0, audioListeners = 0;

        foreach (var go in FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (go.GetComponent<MeshCollider>() != null) meshColliders++;
            var al = go.GetComponent<AudioListener>();
            if (al != null && al.enabled) audioListeners++;
            var light = go.GetComponent<Light>();
            if (light != null && light.enabled)
            {
                if (light.lightmapBakeType == LightmapBakeType.Realtime) realtimeLights++;
                if (light.shadows != LightShadows.None) shadowLights++;
            }
        }

        sb.AppendLine($"MeshColliders: {meshColliders} {(meshColliders > 1000 ? "WARNING >1000" : "OK")}");
        sb.AppendLine($"Realtime Lights: {realtimeLights} {(realtimeLights > 100 ? "WARNING >100" : "OK")}");
        sb.AppendLine($"Shadow Lights: {shadowLights} {(shadowLights > 10 ? "WARNING >10" : "OK")}");
        sb.AppendLine($"AudioListeners: {audioListeners} {(audioListeners != 1 ? $"WARNING: need exactly 1, found {audioListeners}" : "OK")}");

        sb.AppendLine("\nRECOMMENDATIONS:");
        if (meshColliders > 300) sb.AppendLine("- Replace MeshColliders on tiles/walls with Box/CapsuleColliders");
        if (realtimeLights > 100) sb.AppendLine("- Bake decorative lights, keep <100 realtime");
        if (shadowLights > 10) sb.AppendLine("- Disable shadows on all but 1-3 key lights");
        if (audioListeners != 1) sb.AppendLine("- Disable extra AudioListeners - keep only one on Main Camera");

        _report = sb.ToString();
        Debug.Log(_report);
    }

    void OnGUI()
    {
        if (GUILayout.Button("Run Validation")) RunValidation();
        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        EditorGUILayout.TextArea(_report, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
    }
}
#endif
