using System.Collections.Generic;
using UnityEngine;

public class NH_AreaControlManager : MonoBehaviour
{
    public static NH_AreaControlManager Instance { get; private set; }

    private Dictionary<string, NH_Zone> _zones = new Dictionary<string, NH_Zone>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        foreach (var zone in FindObjectsByType<NH_Zone>(FindObjectsSortMode.None))
            _zones[zone.zoneName] = zone;
    }

    public NH_Zone GetZone(string zoneName)
    {
        _zones.TryGetValue(zoneName, out var z);
        return z;
    }

    public void SetZoneControl(string zoneName, NH_AreaControlState state, string factionId = "")
    {
        if (_zones.TryGetValue(zoneName, out var zone))
            zone.SetControl(state, factionId);
    }

    public void NotifyPlayerEnteredZone(NH_PlayerStats player, NH_Zone zone)
    {
        if (zone.isRestricted)
            NH_GuardSuspicionSystem.Instance?.AddSuspicion(zone.suspicionOnEnter, "restricted_zone");
    }

    public List<NH_Zone> GetZonesControlledBy(string factionId)
    {
        var result = new List<NH_Zone>();
        foreach (var z in _zones.Values)
            if (z.controlState == NH_AreaControlState.FactionControlled && z.controllingFactionId == factionId)
                result.Add(z);
        return result;
    }
}
