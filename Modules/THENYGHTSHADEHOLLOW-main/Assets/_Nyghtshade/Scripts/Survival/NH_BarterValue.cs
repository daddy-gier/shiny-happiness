using UnityEngine;

[System.Serializable]
public class NH_BarterValue
{
    public NH_ItemDefinition item;
    public float factionMultiplier = 1f;
    public float scarcityBonus;
    public bool isHot;

    public float FinalValue()
    {
        if (item == null) return 0f;
        float val = item.barterValue * factionMultiplier;
        if (isHot) val *= 1.5f;
        return val + scarcityBonus;
    }
}
