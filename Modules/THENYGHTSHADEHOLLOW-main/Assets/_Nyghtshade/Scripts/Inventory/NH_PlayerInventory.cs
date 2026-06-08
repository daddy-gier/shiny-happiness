using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_PlayerInventory : MonoBehaviour
{
    public int maxSlots = 20;
    public List<NH_InventoryItem> items = new List<NH_InventoryItem>();

    public event Action OnInventoryChanged;

    public bool HasItem(string itemId)
    {
        return items.Exists(i => i.definition != null && i.definition.itemId == itemId);
    }

    public NH_InventoryItem GetItem(string itemId)
    {
        return items.Find(i => i.definition != null && i.definition.itemId == itemId);
    }

    public bool AddItem(NH_ItemDefinition def, int qty = 1)
    {
        if (def == null) return false;

        if (def.stackable)
        {
            var existing = GetItem(def.itemId);
            if (existing != null)
            {
                existing.quantity = Mathf.Min(existing.quantity + qty, def.maxStack);
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        if (items.Count >= maxSlots) return false;
        items.Add(new NH_InventoryItem(def, qty));
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool RemoveItem(string itemId, int qty = 1)
    {
        var item = GetItem(itemId);
        if (item == null) return false;

        item.quantity -= qty;
        if (item.quantity <= 0) items.Remove(item);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public float GetTotalHeat()
    {
        float total = 0f;
        foreach (var item in items) total += item.TotalHeat;
        return total;
    }

    public List<NH_InventoryItem> GetContrabandItems()
    {
        return items.FindAll(i => i.IsContraband);
    }

    public void ConfiscateAll()
    {
        items.RemoveAll(i => i.definition != null && i.definition.canBeConfiscated && i.IsContraband);
        OnInventoryChanged?.Invoke();
    }
}
