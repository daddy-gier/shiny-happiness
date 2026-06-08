#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Text;

public class NH_BuildReadinessReport : EditorWindow
{
    [MenuItem("Nyghtshade/Generate Build Readiness Report")]
    public static void GenerateReport()
    {
        var window = GetWindow<NH_BuildReadinessReport>("Build Readiness");
        window.Show();
        window.RunCheck();
    }

    private string _report = "";
    private Vector2 _scroll;

    void RunCheck()
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== NYGHTSHADE BUILD READINESS ===\n");

        Check(sb, "Player (NH_PlayerController)", FindFirstObjectByType<NH_PlayerController>() != null);
        Check(sb, "Player Stats", FindFirstObjectByType<NH_PlayerStats>() != null);
        Check(sb, "Camera Bootstrap", FindFirstObjectByType<NH_CameraBootstrap>() != null);
        Check(sb, "Game Bootstrap", FindFirstObjectByType<NH_GameBootstrap>() != null);
        Check(sb, "Interaction System", FindFirstObjectByType<NH_InteractionSystem>() != null);
        Check(sb, "Player Inventory", FindFirstObjectByType<NH_PlayerInventory>() != null);
        Check(sb, "Story Flag Manager", FindFirstObjectByType<NH_StoryFlagManager>() != null);
        Check(sb, "Quest Manager", FindFirstObjectByType<NH_QuestManager>() != null);
        Check(sb, "Faction Manager", FindFirstObjectByType<NH_FactionManager>() != null);
        Check(sb, "Schedule Manager", FindFirstObjectByType<NH_PrisonScheduleManager>() != null);
        Check(sb, "Lockdown Manager", FindFirstObjectByType<NH_LockdownManager>() != null);
        Check(sb, "Guard Suspicion System", FindFirstObjectByType<NH_GuardSuspicionSystem>() != null);
        Check(sb, "Contraband System", FindFirstObjectByType<NH_ContrabandSystem>() != null);
        Check(sb, "Riot Manager", FindFirstObjectByType<NH_RiotManager>() != null);
        Check(sb, "GRAVE Manager", FindFirstObjectByType<NH_GRAVEManager>() != null);
        Check(sb, "Save Manager", FindFirstObjectByType<NH_SaveManager>() != null);
        Check(sb, "UI Manager", FindFirstObjectByType<NH_UIManager>() != null);
        Check(sb, "Audio Manager", FindFirstObjectByType<NH_AudioManager>() != null);
        Check(sb, "Area Control Manager", FindFirstObjectByType<NH_AreaControlManager>() != null);
        Check(sb, "MMA Fight Club", FindFirstObjectByType<NH_MMAFightClubSystem>() != null);
        Check(sb, "Basketball Gym", FindFirstObjectByType<NH_BasketballGymSystem>() != null);
        Check(sb, "Morgue Evidence", FindFirstObjectByType<NH_MorgueEvidence>() != null);
        Check(sb, "Tunnel Entrance", FindFirstObjectByType<NH_TunnelEntrance>() != null);
        Check(sb, "Prison Economy Manager", FindFirstObjectByType<NH_PrisonEconomyManager>() != null);
        Check(sb, "Spawn Point", FindFirstObjectByType<NH_PlayerSpawnPoint>() != null);
        Check(sb, "At Least One Zone", FindFirstObjectByType<NH_Zone>() != null);
        Check(sb, "At Least One Door", FindFirstObjectByType<NH_DoorController>() != null);

        int mainCams = 0;
        foreach (var c in FindObjectsByType<Camera>(FindObjectsSortMode.None))
            if (c.CompareTag("MainCamera")) mainCams++;
        Check(sb, "Single Main Camera", mainCams == 1);

        _report = sb.ToString();
        Debug.Log(_report);
    }

    void Check(StringBuilder sb, string label, bool pass)
    {
        sb.AppendLine($"{(pass ? "[ OK ]" : "[MISS]")} {label}");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Run Check")) RunCheck();
        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        EditorGUILayout.TextArea(_report, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
    }
}
#endif
