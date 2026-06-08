using UnityEngine;
using System;

public class NH_ContrabandSystem : MonoBehaviour
{
    public static NH_ContrabandSystem Instance { get; private set; }

    [Range(0f, 100f)] public float globalHeatLevel;

    public event Action<float> OnHeatChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public float GetPlayerHeat()
    {
        var inv = FindFirstObjectByType<NH_PlayerInventory>();
        if (inv == null) return 0f;
        return inv.GetTotalHeat();
    }

    public void AddGlobalHeat(float amount)
    {
        globalHeatLevel = Mathf.Min(100f, globalHeatLevel + amount);
        OnHeatChanged?.Invoke(globalHeatLevel);
    }

    public void ReduceGlobalHeat(float amount)
    {
        globalHeatLevel = Mathf.Max(0f, globalHeatLevel - amount);
        OnHeatChanged?.Invoke(globalHeatLevel);
    }

    public bool IsHotZone() => globalHeatLevel > 60f;

    public NH_SearchRisk EvaluateSearchRisk(NH_PlayerInventory inventory)
    {
        float heat = inventory.GetTotalHeat();
        if (heat == 0f) return NH_SearchRisk.None;
        if (heat < 20f) return NH_SearchRisk.Low;
        if (heat < 50f) return NH_SearchRisk.Medium;
        return NH_SearchRisk.High;
    }
}

public enum NH_SearchRisk { None, Low, Medium, High }
