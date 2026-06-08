using System;
using UnityEngine;

public class NH_GuardSuspicionSystem : MonoBehaviour
{
    public static NH_GuardSuspicionSystem Instance { get; private set; }

    [Range(0f, 100f)] public float globalSuspicion;
    public float suspicionDecayRate = 1f;

    public NH_GuardResponseLevel responseLevel = NH_GuardResponseLevel.Level0_NormalPatrol;

    public event Action<float> OnSuspicionChanged;
    public event Action<NH_GuardResponseLevel> OnResponseLevelChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update()
    {
        if (globalSuspicion > 0f)
        {
            globalSuspicion = Mathf.Max(0f, globalSuspicion - suspicionDecayRate * Time.deltaTime);
            OnSuspicionChanged?.Invoke(globalSuspicion);
            UpdateResponseLevel();
        }
    }

    public void AddSuspicion(float amount, string reason = "")
    {
        globalSuspicion = Mathf.Min(100f, globalSuspicion + amount);
        if (!string.IsNullOrEmpty(reason))
            Debug.Log($"[NH Security] Suspicion +{amount} ({reason}). Total: {globalSuspicion:F1}");
        OnSuspicionChanged?.Invoke(globalSuspicion);
        UpdateResponseLevel();
    }

    void UpdateResponseLevel()
    {
        NH_GuardResponseLevel newLevel;
        if (globalSuspicion < 20f) newLevel = NH_GuardResponseLevel.Level0_NormalPatrol;
        else if (globalSuspicion < 40f) newLevel = NH_GuardResponseLevel.Level1_ExtraGuards;
        else if (globalSuspicion < 60f) newLevel = NH_GuardResponseLevel.Level2_Lockdown;
        else if (globalSuspicion < 80f) newLevel = NH_GuardResponseLevel.Level3_CellBlockSweep;
        else if (globalSuspicion < 95f) newLevel = NH_GuardResponseLevel.Level4_TacticalStaged;
        else newLevel = NH_GuardResponseLevel.Level5_BreachResolution;

        if (newLevel != responseLevel)
        {
            responseLevel = newLevel;
            OnResponseLevelChanged?.Invoke(responseLevel);
        }
    }
}
