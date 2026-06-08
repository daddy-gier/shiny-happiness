using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NH_InventoryUI : MonoBehaviour
{
    public Transform itemListParent;
    public GameObject itemRowPrefab;

    private NH_PlayerInventory _inventory;

    void OnEnable()
    {
        _inventory = FindFirstObjectByType<NH_PlayerInventory>();
        if (_inventory != null)
        {
            _inventory.OnInventoryChanged += Refresh;
            Refresh();
        }
    }

    void OnDisable()
    {
        if (_inventory != null) _inventory.OnInventoryChanged -= Refresh;
    }

    void Refresh()
    {
        if (itemListParent == null || itemRowPrefab == null || _inventory == null) return;

        foreach (Transform child in itemListParent) Destroy(child.gameObject);

        foreach (var item in _inventory.items)
        {
            var row = Instantiate(itemRowPrefab, itemListParent);
            var label = row.GetComponentInChildren<TMP_Text>();
            if (label)
                label.text = $"{item.definition?.displayName ?? "?"} x{item.quantity}  [{item.definition?.heatCategory}]";
        }
    }
}
