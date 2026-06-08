using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NH_RestrictedZone : MonoBehaviour
{
    public float suspicionOnEnter = 25f;
    public bool triggerAlarmOnEnter;
    public string alarmReason = "restricted_zone_breach";

    void Awake() => GetComponent<Collider>().isTrigger = true;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        NH_GuardSuspicionSystem.Instance?.AddSuspicion(suspicionOnEnter, alarmReason);

        var stats = other.GetComponentInParent<NH_PlayerStats>();
        stats?.AddSuspicion(suspicionOnEnter);

        if (triggerAlarmOnEnter)
            NH_AlarmSystem.Instance?.TriggerAlarm(alarmReason);
    }
}
