using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class NH_Zone : MonoBehaviour
{
    [Header("Zone Identity")]
    public string zoneName;
    public NH_ZoneType zoneType;

    [Header("Access")]
    public bool isRestricted;
    public NH_KeycardAccessLevel requiredAccess;
    public float suspicionOnEnter;

    [Header("Control")]
    public NH_AreaControlState controlState = NH_AreaControlState.Neutral;
    public string controllingFactionId;

    [Header("Atmosphere")]
    public float noiseLevel;
    public float dangerLevel;
    public float stressModifier;

    public event Action<NH_PlayerStats, NH_Zone> OnPlayerEnter;
    public event Action<NH_PlayerStats, NH_Zone> OnPlayerExit;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        var stats = other.GetComponentInParent<NH_PlayerStats>();
        if (stats == null) return;

        stats.CurrentZone = zoneName;
        if (suspicionOnEnter > 0f) stats.AddSuspicion(suspicionOnEnter);
        if (stressModifier > 0f) stats.AddStress(stressModifier);

        NH_AreaControlManager.Instance?.NotifyPlayerEnteredZone(stats, this);
        OnPlayerEnter?.Invoke(stats, this);
    }

    void OnTriggerExit(Collider other)
    {
        var stats = other.GetComponentInParent<NH_PlayerStats>();
        if (stats == null) return;
        OnPlayerExit?.Invoke(stats, this);
    }

    public void SetControl(NH_AreaControlState state, string factionId = "")
    {
        controlState = state;
        controllingFactionId = factionId;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isRestricted ? new Color(1f, 0f, 0f, 0.25f) : new Color(0f, 1f, 1f, 0.15f);
        Bounds b = GetComponent<Collider>().bounds;
        Gizmos.DrawCube(b.center, b.size);
    }
}
