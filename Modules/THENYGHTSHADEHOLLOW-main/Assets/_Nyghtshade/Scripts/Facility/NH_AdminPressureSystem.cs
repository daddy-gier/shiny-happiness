using UnityEngine;

public class NH_AdminPressureSystem : MonoBehaviour
{
    public static NH_AdminPressureSystem Instance { get; private set; }

    [Range(0f, 100f)] public float scandalHeat;
    [Range(0f, 100f)] public float budgetPressure;
    [Range(0f, 100f)] public float mediaHeat;
    [Range(0f, 100f)] public float lawsuitRisk;
    [Range(0f, 100f)] public float guardUnionPressure;
    [Range(0f, 100f)] public float reformProgress;
    [Range(0f, 100f)] public float corruptionDepth;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void AddScandalHeat(float amount)
    {
        scandalHeat = Mathf.Min(100f, scandalHeat + amount);
        if (scandalHeat > 80f)
            NH_RiotManager.Instance?.AddRiotTrigger("admin_scandal");
    }

    public void ReduceCorruption(float amount)
    {
        corruptionDepth = Mathf.Max(0f, corruptionDepth - amount);
        reformProgress = Mathf.Min(100f, reformProgress + amount * 0.5f);
    }
}
