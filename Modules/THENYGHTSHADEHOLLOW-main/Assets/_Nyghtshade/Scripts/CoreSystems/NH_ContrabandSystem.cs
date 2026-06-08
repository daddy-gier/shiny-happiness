using UnityEngine;
using System.Collections.Generic;

public enum NH_ContrabandCategory
{
    None,
    Legal,
    LowHeat,
    MediumHeat,
    HighHeat,
    Evidence,
    GRAVERelic
}

public class NH_ContrabandSystem : MonoBehaviour
{
    public static NH_ContrabandSystem Instance { get; private set; }

    [Header("Heat Values")]
    public float LowHeatValue = 10f;
    public float MediumHeatValue = 30f;
    public float HighHeatValue = 75f;
    public float EvidenceHeatValue = 50f;
    public float GRAVERelicHeatValue = 90f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public float CalculateTotalInventoryHeat(List<NH_ContrabandCategory> items)
    {
        if (items == null) return 0f;
        float score = 0f;
        foreach (var item in items)
            score += GetHeatValue(item);
        return Mathf.Clamp(score, 0f, 999f);
    }

    public float GetHeatValue(NH_ContrabandCategory category)
    {
        switch (category)
        {
            case NH_ContrabandCategory.LowHeat:    return LowHeatValue;
            case NH_ContrabandCategory.MediumHeat: return MediumHeatValue;
            case NH_ContrabandCategory.HighHeat:   return HighHeatValue;
            case NH_ContrabandCategory.Evidence:   return EvidenceHeatValue;
            case NH_ContrabandCategory.GRAVERelic: return GRAVERelicHeatValue;
            default:                               return 0f;
        }
    }

    public bool IsIllegal(NH_ContrabandCategory category)
    {
        return category == NH_ContrabandCategory.LowHeat ||
               category == NH_ContrabandCategory.MediumHeat ||
               category == NH_ContrabandCategory.HighHeat ||
               category == NH_ContrabandCategory.GRAVERelic;
    }
}
