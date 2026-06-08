using System;
using UnityEngine;

public class NH_WardenPolicySystem : MonoBehaviour
{
    public static NH_WardenPolicySystem Instance { get; private set; }

    public enum WardenStance { Neutral, Reform, Corruption, Lockdown, Negotiation }

    public WardenStance currentStance = WardenStance.Neutral;
    public bool lockdownAuthorized;
    public bool coverupActive;
    public bool inmateCommitteeActive;
    public float riotRisk;

    public event Action<WardenStance> OnStanceChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SetStance(WardenStance stance)
    {
        currentStance = stance;
        OnStanceChanged?.Invoke(stance);

        if (stance == WardenStance.Lockdown)
            NH_LockdownManager.Instance?.ActivateLockdown("warden_order", 60f);
    }

    public void AuthorizeLockdown()
    {
        lockdownAuthorized = true;
        SetStance(WardenStance.Lockdown);
    }

    public void ActivateCoverup()
    {
        coverupActive = true;
        NH_AdminPressureSystem.Instance?.AddScandalHeat(10f);
    }
}
