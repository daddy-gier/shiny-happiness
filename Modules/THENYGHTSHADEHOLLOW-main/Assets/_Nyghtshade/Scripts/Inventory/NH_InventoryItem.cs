using System;

[Serializable]
public class NH_InventoryItem
{
    public NH_ItemDefinition definition;
    public int quantity;
    public bool isHidden;

    public NH_InventoryItem(NH_ItemDefinition def, int qty = 1)
    {
        definition = def;
        quantity = qty;
    }

    public float TotalHeat => definition != null ? definition.heatValue * quantity : 0f;
    public bool IsContraband => definition != null &&
        (definition.heatCategory == NH_ItemHeatCategory.ContrabandLow ||
         definition.heatCategory == NH_ItemHeatCategory.ContrabandMedium ||
         definition.heatCategory == NH_ItemHeatCategory.ContrabandHigh ||
         definition.heatCategory == NH_ItemHeatCategory.WeaponFictional);
}
