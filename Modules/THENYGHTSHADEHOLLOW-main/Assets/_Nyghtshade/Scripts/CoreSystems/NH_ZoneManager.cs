using UnityEngine;
using System.Collections.Generic;

public enum NH_ZoneType
{
    Unknown,
    CellBlock,
    Yard,
    Morgue,
    MMAWarehouse,
    BasketballGym,
    RestrictedAdmin,
    Cafeteria,
    Chapel,
    Infirmary,
    Kitchen,
    GRAVETunnel
}

public enum NH_ZoneControlState
{
    Guards,
    Neutral,
    FactionControlled,
    RiotCrowd,
    LockdownSealed,
    GRAVEInfluence
}

public class NH_ZoneManager : MonoBehaviour
{
    public static NH_ZoneManager Instance { get; private set; }

    private readonly Dictionary<NH_ZoneType, NH_ZoneControlState> zoneControl =
        new Dictionary<NH_ZoneType, NH_ZoneControlState>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InitializeDefaults();
    }

    private void InitializeDefaults()
    {
        foreach (NH_ZoneType zone in System.Enum.GetValues(typeof(NH_ZoneType)))
        {
            if (zone == NH_ZoneType.Unknown) continue;
            if (!zoneControl.ContainsKey(zone))
                zoneControl.Add(zone, NH_ZoneControlState.Guards);
        }
    }

    public NH_ZoneControlState GetCurrentControl(NH_ZoneType zone)
    {
        return zoneControl.TryGetValue(zone, out var state) ? state : NH_ZoneControlState.Neutral;
    }

    public void SetZoneControl(NH_ZoneType zone, NH_ZoneControlState state)
    {
        zoneControl[zone] = state;
        Debug.Log($"[ZONE CONTROL] {zone} is now {state}");
    }

    public bool IsZoneLockedDown(NH_ZoneType zone)
    {
        return GetCurrentControl(zone) == NH_ZoneControlState.LockdownSealed;
    }

    public void LockdownAllZones()
    {
        var keys = new List<NH_ZoneType>(zoneControl.Keys);
        foreach (var key in keys)
            zoneControl[key] = NH_ZoneControlState.LockdownSealed;
        Debug.LogWarning("[ZONE CONTROL] All zones set to LockdownSealed.");
    }
}
