using UnityEngine;

public class NH_TacticalResponseSystem : MonoBehaviour
{
    public static NH_TacticalResponseSystem Instance { get; private set; }

    public NH_GuardResponseLevel responseLevel = NH_GuardResponseLevel.Level0_NormalPatrol;
    public bool gasDeployed;
    public bool exteriorForceStaged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void EscalateResponse(NH_GuardResponseLevel level)
    {
        responseLevel = level;

        switch (level)
        {
            case NH_GuardResponseLevel.Level2_Lockdown:
                NH_LockdownManager.Instance?.ActivateLockdown("tactical_response", 60f);
                break;
            case NH_GuardResponseLevel.Level4_TacticalStaged:
                exteriorForceStaged = true;
                break;
            case NH_GuardResponseLevel.Level5_BreachResolution:
                DeployGas();
                break;
        }
    }

    void DeployGas()
    {
        gasDeployed = true;
        foreach (var zone in FindObjectsByType<NH_GasExposureZone>(FindObjectsSortMode.None))
            zone.Activate();
        Debug.Log("[NH] Tactical gas deployed.");
    }

    public void StandDown()
    {
        responseLevel = NH_GuardResponseLevel.Level0_NormalPatrol;
        gasDeployed = false;
        exteriorForceStaged = false;
        foreach (var zone in FindObjectsByType<NH_GasExposureZone>(FindObjectsSortMode.None))
            zone.Deactivate();
    }
}
