using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_StashPoint : NH_InteractableBase
{
    public enum StashType { PersonalCellStash, CommonAreaStash, YardStash, KitchenLaundryStash, ChapelStash, MedicalStash, TunnelStash, FactionCache }

    [Header("Stash Config")]
    public StashType stashType;
    public NH_StashQuality quality = NH_StashQuality.Basic;
    public int capacity = 5;
    [Range(0f, 1f)] public float discoveryRisk = 0.1f;
    [Range(0f, 1f)] public float degradationRisk = 0.05f;
    public string ownerFactionId;

    private List<NH_InventoryItem> _contents = new List<NH_InventoryItem>();

    public override string InteractVerb => "Search Stash";

    public override void Interact(NH_PlayerStats player)
    {
        Debug.Log($"[NH] Accessing {stashType} stash. {_contents.Count}/{capacity} items stored.");
    }

    public bool StoreItem(NH_InventoryItem item)
    {
        if (_contents.Count >= capacity) return false;
        _contents.Add(item);
        return true;
    }

    public NH_InventoryItem RetrieveItem(string itemId)
    {
        var item = _contents.Find(i => i.definition.itemId == itemId);
        if (item != null) _contents.Remove(item);
        return item;
    }

    public bool CheckDiscovery()
    {
        float risk = discoveryRisk;
        if (quality == NH_StashQuality.Hidden) risk *= 0.3f;
        else if (quality == NH_StashQuality.Fortified) risk *= 0.1f;
        else if (quality == NH_StashQuality.Poor) risk *= 2f;
        return UnityEngine.Random.value < risk;
    }
}
