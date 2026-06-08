using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_FactionManager : MonoBehaviour
{
    public static NH_FactionManager Instance { get; private set; }

    [Header("Faction Definitions")]
    public NH_FactionDefinition[] factionDefinitions;

    private Dictionary<string, NH_FactionStanding> _standings = new Dictionary<string, NH_FactionStanding>();
    private Dictionary<string, NH_FactionRiotStance> _riotStances = new Dictionary<string, NH_FactionRiotStance>();

    public event Action<string, float> OnReputationChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        foreach (var def in factionDefinitions)
            _standings[def.factionId] = new NH_FactionStanding(def.factionId);
    }

    public NH_FactionStanding GetStanding(string factionId)
    {
        if (!_standings.TryGetValue(factionId, out var s))
        {
            s = new NH_FactionStanding(factionId);
            _standings[factionId] = s;
        }
        return s;
    }

    public void ModifyRespect(string factionId, float delta)
    {
        GetStanding(factionId).ModifyRespect(delta);
        OnReputationChanged?.Invoke(factionId, delta);
    }

    public void ModifyFear(string factionId, float delta) => GetStanding(factionId).ModifyFear(delta);
    public void ModifyTrust(string factionId, float delta) => GetStanding(factionId).ModifyTrust(delta);
    public void ModifyDebt(string factionId, float delta) => GetStanding(factionId).ModifyDebt(delta);

    public void SetRiotStance(string factionId, NH_FactionRiotStance stance) => _riotStances[factionId] = stance;
    public NH_FactionRiotStance GetRiotStance(string factionId)
    {
        _riotStances.TryGetValue(factionId, out var s);
        return s;
    }

    public bool IsHostile(string factionId) => GetStanding(factionId).isHostile;
    public bool IsProtecting(string factionId) => GetStanding(factionId).isProtected;
}
