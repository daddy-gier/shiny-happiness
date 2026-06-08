using UnityEngine;

public enum NH_GuardResponseLevel
{
    Normal,
    ExtraPatrols,
    Lockdown,
    Sweep,
    TacticalStaged,
    Resolution
}

public class NH_LockdownManager : MonoBehaviour
{
    public static NH_LockdownManager Instance { get; private set; }

    public bool IsLockdownActive { get; private set; }
    public float SecuritySuspicionLevel = 0f;
    public NH_GuardResponseLevel CurrentResponseLevel = NH_GuardResponseLevel.Normal;
    public string LastLockdownReason = "None";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void TriggerLockdown(string reason)
    {
        IsLockdownActive = true;
        LastLockdownReason = string.IsNullOrWhiteSpace(reason) ? "Unspecified Security Event" : reason;
        CurrentResponseLevel = NH_GuardResponseLevel.Lockdown;

        if (NH_ZoneManager.Instance != null)
            NH_ZoneManager.Instance.LockdownAllZones();

        Debug.LogWarning($"[LOCKDOWN ACTIVATED] Reason: {LastLockdownReason}. All zone rules shifting.");
    }

    public void EndLockdown()
    {
        IsLockdownActive = false;
        CurrentResponseLevel = NH_GuardResponseLevel.Normal;
        SecuritySuspicionLevel = Mathf.Min(SecuritySuspicionLevel, 50f);
        Debug.Log("[LOCKDOWN ENDED] Returning to normal guard response.");
    }

    public void AlterSuspicion(float amount)
    {
        SecuritySuspicionLevel = Mathf.Clamp(SecuritySuspicionLevel + amount, 0f, 100f);

        if (SecuritySuspicionLevel >= 90f && !IsLockdownActive)
            TriggerLockdown("High Suspicion Threshold Reached");
        else if (SecuritySuspicionLevel >= 70f)
            CurrentResponseLevel = NH_GuardResponseLevel.ExtraPatrols;

        Debug.Log($"[SUSPICION] SecuritySuspicionLevel = {SecuritySuspicionLevel}");
    }
}
