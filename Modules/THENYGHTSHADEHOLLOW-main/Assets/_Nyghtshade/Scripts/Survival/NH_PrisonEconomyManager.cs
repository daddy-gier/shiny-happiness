using System.Collections.Generic;
using UnityEngine;

public class NH_PrisonEconomyManager : MonoBehaviour
{
    public static NH_PrisonEconomyManager Instance { get; private set; }

    [Header("Economy State")]
    public float scarcityMultiplier = 1f;
    public float blackMarketHeatMultiplier = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public float GetBarterValue(NH_ItemDefinition item, string factionId = "")
    {
        if (item == null) return 0f;
        float base_ = item.barterValue * scarcityMultiplier;

        var standing = NH_FactionManager.Instance?.GetStanding(factionId);
        if (standing != null && standing.respect > 50f) base_ *= 0.85f;

        return base_;
    }

    public float GetBlackMarketValue(NH_ItemDefinition item)
    {
        if (item == null) return 0f;
        float heatPremium = 1f + (item.heatValue / 100f) * blackMarketHeatMultiplier;
        return item.blackMarketValue * heatPremium * scarcityMultiplier;
    }
}
