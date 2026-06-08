using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NH_SaveManager : MonoBehaviour
{
    public static NH_SaveManager Instance { get; private set; }

    private string SavePath => Path.Combine(Application.persistentDataPath, "nyghtshade_save.json");

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) SaveGame();
        if (Input.GetKeyDown(KeyCode.F9)) LoadGame();
    }

    public void SaveGame()
    {
        var data = new NH_SaveData();
        data.saveDateTimeUtc = DateTime.UtcNow.ToString("o");

        var player = FindFirstObjectByType<NH_PlayerStats>();
        if (player != null)
        {
            data.playerPosX = player.transform.position.x;
            data.playerPosY = player.transform.position.y;
            data.playerPosZ = player.transform.position.z;
            data.playerRotY = player.transform.eulerAngles.y;
            data.health = player.Health;
            data.stamina = player.Stamina;
            data.stress = player.Stress;
            data.suspicion = player.SuspicionExposure;
        }

        var sched = NH_PrisonScheduleManager.Instance;
        if (sched != null) { data.currentDay = sched.CurrentDay; data.currentScheduleBlock = sched.CurrentBlockName; }

        var inv = FindFirstObjectByType<NH_PlayerInventory>();
        if (inv != null)
        {
            foreach (var item in inv.items)
            {
                data.inventoryItemIds.Add(item.definition?.itemId ?? "");
                data.inventoryQuantities.Add(item.quantity);
            }
        }

        var contraband = NH_ContrabandSystem.Instance;
        if (contraband != null) data.globalHeat = contraband.globalHeatLevel;

        var susp = NH_GuardSuspicionSystem.Instance;
        if (susp != null) data.globalSuspicion = susp.globalSuspicion;

        var lockdown = NH_LockdownManager.Instance;
        if (lockdown != null) { data.lockdownActive = lockdown.isLockdownActive; data.lockdownReason = lockdown.lockdownReason; }

        var riot = NH_RiotManager.Instance;
        if (riot != null) { data.riotActive = riot.isActive; data.riotPhase = (int)riot.currentPhase; data.riotThreat = riot.riotThreat; }

        var flags = NH_StoryFlagManager.Instance;
        if (flags != null) data.storyFlags = flags.GetAllFlags();

        var quests = NH_QuestManager.Instance;
        if (quests != null)
        {
            foreach (var q in quests.allQuests)
            {
                data.questIds.Add(q.questId);
                data.questStates.Add((int)quests.GetState(q.questId));
            }
        }

        var factions = NH_FactionManager.Instance;
        if (factions != null)
        {
            foreach (var def in factions.factionDefinitions)
            {
                data.factionIds.Add(def.factionId);
                data.factionRespect.Add(factions.GetStanding(def.factionId).respect);
            }
        }

        var grave = NH_GRAVEManager.Instance;
        if (grave != null) { data.graveClues = grave.GetDiscoveredClues(); data.graveMysteryDepth = grave.mysteryDepth; }

        var mma = NH_MMAFightClubSystem.Instance;
        if (mma != null) { data.fightClubReputation = mma.fightClubReputation; data.playerFightRank = mma.playerRank; }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[NH] Game saved to {SavePath}");
    }

    public void LoadGame()
    {
        if (!File.Exists(SavePath)) { Debug.Log("[NH] No save file found."); return; }

        string json = File.ReadAllText(SavePath);
        var data = JsonUtility.FromJson<NH_SaveData>(json);

        var player = FindFirstObjectByType<NH_PlayerController>();
        if (player != null)
            player.Teleport(new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ),
                Quaternion.Euler(0f, data.playerRotY, 0f));

        var stats = FindFirstObjectByType<NH_PlayerStats>();
        if (stats != null)
        {
            stats.health = data.health;
            stats.stamina = data.stamina;
            stats.stress = data.stress;
        }

        NH_StoryFlagManager.Instance?.LoadFlags(data.storyFlags);

        var riot = NH_RiotManager.Instance;
        if (riot != null) { riot.isActive = data.riotActive; riot.riotThreat = data.riotThreat; riot.currentPhase = (NH_RiotPhase)data.riotPhase; }

        if (NH_ContrabandSystem.Instance != null) NH_ContrabandSystem.Instance.globalHeatLevel = data.globalHeat;

        if (NH_MMAFightClubSystem.Instance != null)
        {
            NH_MMAFightClubSystem.Instance.fightClubReputation = data.fightClubReputation;
            NH_MMAFightClubSystem.Instance.playerRank = data.playerFightRank;
        }

        Debug.Log("[NH] Game loaded.");
    }

    public void NewGame()
    {
        if (File.Exists(SavePath)) File.Delete(SavePath);
        Debug.Log("[NH] New game started.");
    }
}
