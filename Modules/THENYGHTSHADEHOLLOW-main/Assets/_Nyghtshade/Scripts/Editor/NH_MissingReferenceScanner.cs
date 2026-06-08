#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Text;

public class NH_MissingReferenceScanner : EditorWindow
{
    [MenuItem("Nyghtshade/Scan Missing References")]
    public static void Scan()
    {
        var window = GetWindow<NH_MissingReferenceScanner>("Missing Refs");
        window.Show();
        window.RunScan();
    }

    private string _report = "";
    private Vector2 _scroll;

    void RunScan()
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== MISSING REFERENCE SCAN ===\n");

        int totalMissing = 0;
        foreach (var go in FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            foreach (var comp in go.GetComponents<Component>())
            {
                if (comp == null)
                {
                    sb.AppendLine($"Missing script on: {GetPath(go)}");
                    totalMissing++;
                    continue;
                }

                var so = new SerializedObject(comp);
                var prop = so.GetIterator();
                while (prop.NextVisible(true))
                {
                    if (prop.propertyType == SerializedPropertyType.ObjectReference &&
                        prop.objectReferenceValue == null &&
                        prop.objectReferenceInstanceIDValue != 0)
                    {
                        sb.AppendLine($"Missing ref '{prop.name}' on {comp.GetType().Name} @ {GetPath(go)}");
                        totalMissing++;
                    }
                }
            }
        }

        sb.Insert(0, $"Total missing: {totalMissing}\n\n");
        _report = sb.ToString();
        Debug.Log(_report);
    }

    string GetPath(GameObject go)
    {
        string path = go.name;
        Transform t = go.transform.parent;
        while (t != null) { path = t.name + "/" + path; t = t.parent; }
        return path;
    }

    void OnGUI()
    {
        if (GUILayout.Button("Run Scan")) RunScan();
        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        EditorGUILayout.TextArea(_report, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
    }
}
#endif
