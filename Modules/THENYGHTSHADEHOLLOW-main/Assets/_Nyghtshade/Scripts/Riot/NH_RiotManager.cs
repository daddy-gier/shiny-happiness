using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_RiotManager : MonoBehaviour
{
    public static NH_RiotManager Instance { get; private set; }

    [Header("Riot State")]
    public bool isActive;
    public NH_RiotPhase currentPhase = NH_RiotPhase.Phase0_RisingTension;
    [Range(0f, 100f)] public float riotThreat;
    [Range(0f, 100f)] public float playerInvolvementLevel;

    [Header("Triggers Accumulated")]
    public List<NH_RiotTriggerEntry> triggers = new List<NH_RiotTriggerEntry>();

    public event Action<NH_RiotPhase> OnPhaseChanged;
    public event Action OnRiotStarted;
    public event Action OnRiotEnded;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void AddRiotTrigger(string triggerId)
    {
        var entry = triggers.Find(t => t.triggerId == triggerId);
        if (entry == null)
        {
            entry = new NH_RiotTriggerEntry { triggerId = triggerId };
            triggers.Add(entry);
        }

        if (!entry.hasTriggered)
        {
            entry.hasTriggered = true;
            riotThreat = Mathf.Min(100f, riotThreat + entry.weight * 15f);
            Debug.Log($"[NH Riot] Trigger: {triggerId}. Threat: {riotThreat:F0}");
        }

        if (riotThreat >= 100f && !isActive)
            StartRiot();
        else if (riotThreat >= 40f && currentPhase == NH_RiotPhase.Phase0_RisingTension)
            AdvancePhase();
    }

    public void StartRiot()
    {
        isActive = true;
        AdvanceToPhase(NH_RiotPhase.Phase1_Flashpoint);
        OnRiotStarted?.Invoke();
        NH_LockdownManager.Instance?.ActivateLockdown("riot_flashpoint", 90f);
        BroadcastRiotStances();
    }

    public void AdvancePhase()
    {
        int next = Mathf.Min((int)currentPhase + 1, (int)NH_RiotPhase.Phase7_Aftermath);
        AdvanceToPhase((NH_RiotPhase)next);
    }

    void AdvanceToPhase(NH_RiotPhase phase)
    {
        currentPhase = phase;
        OnPhaseChanged?.Invoke(phase);

        if (phase == NH_RiotPhase.Phase7_Aftermath)
            EndRiot();
    }

    void BroadcastRiotStances()
    {
        foreach (var npcRiot in FindObjectsByType<NH_NPCRiotState>(FindObjectsSortMode.None))
        {
            var identity = npcRiot.GetComponent<NH_NPCIdentity>();
            if (identity == null) continue;
            var stance = NH_FactionManager.Instance?.GetRiotStance(identity.factionId) ?? NH_FactionRiotStance.Neutral;
            npcRiot.ApplyRiotStance(stance);
        }
    }

    void EndRiot()
    {
        isActive = false;
        riotThreat = 0f;
        OnRiotEnded?.Invoke();
        NH_LockdownManager.Instance?.EndLockdown();
        NH_TacticalResponseSystem.Instance?.StandDown();
    }
}
