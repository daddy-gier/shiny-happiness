#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Text;

public class NH_OneClickPlayableProof : EditorWindow
{
    [MenuItem("Nyghtshade/Run Performance Counts")]
    public static void RunCounts()
    {
        var window = GetWindow<NH_OneClickPlayableProof>("Performance");
        window.Show();
        window.RunCheck();
    }

    private string _report = "";
    private Vector2 _scroll;

    void RunCheck()
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== PERFORMANCE COUNTS ===\n");

        int total = 0, active = 0, renderers = 0, meshColliders = 0;
        int realtimeLights = 0, bakedLights = 0, animators = 0;

        foreach (var go in FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            total++;
            if (go.activeInHierarchy) active++;
            if (go.GetComponent<MeshRenderer>() != null) renderers++;
            if (go.GetComponent<MeshCollider>() != null) meshColliders++;
            if (go.GetComponent<Animator>() != null) animators++;
            var l = go.GetComponent<Light>();
            if (l != null && l.enabled)
            {
                if (l.lightmapBakeType == LightmapBakeType.Realtime) realtimeLights++;
                else bakedLights++;
            }
        }

        sb.AppendLine($"Total GameObjects: {total}");
        sb.AppendLine($"Active: {active}");
        sb.AppendLine($"MeshRenderers: {renderers}");
        sb.AppendLine($"MeshColliders: {meshColliders}");
        sb.AppendLine($"Animators: {animators}");
        sb.AppendLine($"Realtime Lights: {realtimeLights}");
        sb.AppendLine($"Baked Lights: {bakedLights}");

        sb.AppendLine("\nTARGETS FOR VERTICAL SLICE:");
        sb.AppendLine($"  MeshColliders < 300: {(meshColliders < 300 ? "PASS" : $"FAIL ({meshColliders})")}");
        sb.AppendLine($"  Realtime Lights < 100: {(realtimeLights < 100 ? "PASS" : $"FAIL ({realtimeLights})")}");
        sb.AppendLine($"  Total GOs < 20000: {(total < 20000 ? "PASS" : $"WARN ({total})")}");

        _report = sb.ToString();
        Debug.Log(_report);
    }

    void OnGUI()
    {
        if (GUILayout.Button("Run Counts")) RunCheck();
        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        EditorGUILayout.TextArea(_report, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
    }
}
#endif
