using UnityEngine;

public enum NH_RiotPhase
{
    Phase0_Normal,
    Phase1_Flashpoint,
    Phase2_Surge,
    Phase3_ControlShift,
    Phase4_Siege,
    Phase5_Aftermath
}

public class NH_RiotStateMachine : MonoBehaviour
{
    public static NH_RiotStateMachine Instance { get; private set; }

    public NH_RiotPhase CurrentPhase { get; private set; } = NH_RiotPhase.Phase0_Normal;
    public float RiotThreat = 0f;
    public bool RiotActive => CurrentPhase != NH_RiotPhase.Phase0_Normal &&
                              CurrentPhase != NH_RiotPhase.Phase5_Aftermath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddRiotThreat(float amount, string reason)
    {
        RiotThreat = Mathf.Clamp(RiotThreat + amount, 0f, 100f);
        Debug.LogWarning($"[RIOT THREAT] +{amount}. Reason: {reason}. Total: {RiotThreat}");

        if (RiotThreat >= 25f && CurrentPhase == NH_RiotPhase.Phase0_Normal)
            SetPhase(NH_RiotPhase.Phase1_Flashpoint);
        else if (RiotThreat >= 50f && CurrentPhase == NH_RiotPhase.Phase1_Flashpoint)
            SetPhase(NH_RiotPhase.Phase2_Surge);
        else if (RiotThreat >= 75f && CurrentPhase == NH_RiotPhase.Phase2_Surge)
            SetPhase(NH_RiotPhase.Phase3_ControlShift);
    }

    public void AdvanceRiotPhase()
    {
        if (CurrentPhase == NH_RiotPhase.Phase5_Aftermath) return;
        SetPhase(CurrentPhase + 1);
    }

    public void SetPhase(NH_RiotPhase phase)
    {
        CurrentPhase = phase;
        Debug.LogError($"[RIOT ESCALATION] Scene moving to: {CurrentPhase}");

        if (CurrentPhase == NH_RiotPhase.Phase3_ControlShift)
        {
            if (NH_LockdownManager.Instance != null)
                NH_LockdownManager.Instance.TriggerLockdown("Riot Level 3 Control Shift");

            if (NH_ZoneManager.Instance != null)
            {
                NH_ZoneManager.Instance.SetZoneControl(NH_ZoneType.Yard, NH_ZoneControlState.RiotCrowd);
                NH_ZoneManager.Instance.SetZoneControl(NH_ZoneType.MMAWarehouse, NH_ZoneControlState.RiotCrowd);
                NH_ZoneManager.Instance.SetZoneControl(NH_ZoneType.BasketballGym, NH_ZoneControlState.RiotCrowd);
            }
        }
    }

    public void ResolveRiot()
    {
        CurrentPhase = NH_RiotPhase.Phase5_Aftermath;
        RiotThreat = 0f;
        Debug.LogWarning("[RIOT RESOLVED] Aftermath phase active.");
    }
}
