using UnityEngine;

public class NH_ItemPickup : NH_InteractableBase
{
    public NH_ItemDefinition item;
    public int quantity = 1;

    public override string DisplayName => item != null ? item.displayName : "Unknown Item";
    public override string InteractVerb => "Pick Up";

    public override void Interact(NH_PlayerStats player)
    {
        if (item == null) return;

        var inv = player.GetComponent<NH_PlayerInventory>();
        if (inv == null) return;

        if (inv.AddItem(item, quantity))
        {
            Debug.Log($"[NH] Picked up {quantity}x {item.displayName}");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("[NH] Inventory full.");
        }
    }
}
