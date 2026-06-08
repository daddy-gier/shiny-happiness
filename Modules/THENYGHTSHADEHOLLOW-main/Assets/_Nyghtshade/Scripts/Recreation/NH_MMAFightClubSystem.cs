using System;
using UnityEngine;

public class NH_MMAFightClubSystem : MonoBehaviour
{
    public static NH_MMAFightClubSystem Instance { get; private set; }

    [Header("Fight Club State")]
    public bool isActive;
    public bool playerRegistered;
    public int playerRank;
    public float fightClubReputation;
    [Range(0f, 100f)] public float raidRisk;

    public event Action OnFightClubRaid;
    public event Action<NH_FightResult> OnFightResolved;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void RegisterFighter(NH_PlayerStats player)
    {
        playerRegistered = true;
        NH_StoryFlagManager.Instance.SetFlag("fight_club_registered");
        NH_QuestManager.Instance?.AdvanceQuest("q_fight_club_initiation");
        Debug.Log("[NH] Player registered for fight club.");
    }

    public void StartFight(NH_PlayerStats player, float opponentDifficulty)
    {
        if (!playerRegistered) return;

        var resolver = NH_FightOutcomeResolver.Instance;
        if (resolver == null) return;

        NH_CrowdNoiseSystem.Instance?.SetNoiseLevel(90f);
        var result = resolver.ResolveFight(player, opponentDifficulty);

        if (result == NH_FightResult.Victory)
        {
            playerRank++;
            fightClubReputation = Mathf.Min(100f, fightClubReputation + 10f);
        }

        raidRisk = Mathf.Min(100f, raidRisk + 15f);
        if (raidRisk > 85f) TriggerRaid();

        OnFightResolved?.Invoke(result);
    }

    void TriggerRaid()
    {
        OnFightClubRaid?.Invoke();
        NH_LockdownManager.Instance?.ActivateLockdown("fight_club_raid", 60f);
        raidRisk = 0f;
    }
}
