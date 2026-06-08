using UnityEngine;
using System.Collections.Generic;

public class NH_NPCScheduleAgent : MonoBehaviour
{
    [System.Serializable]
    public class ScheduleEntry
    {
        public string scheduleBlock;
        public Transform destination;
        public NH_NPCState stateOverride;
    }

    public List<ScheduleEntry> schedule = new List<ScheduleEntry>();

    private NH_NPCBrain _brain;

    void Awake() => _brain = GetComponent<NH_NPCBrain>();

    void OnEnable()
    {
        if (NH_PrisonScheduleManager.Instance != null)
            NH_PrisonScheduleManager.Instance.OnScheduleBlockChanged += OnBlockChanged;
    }

    void OnDisable()
    {
        if (NH_PrisonScheduleManager.Instance != null)
            NH_PrisonScheduleManager.Instance.OnScheduleBlockChanged -= OnBlockChanged;
    }

    void OnBlockChanged(string blockName)
    {
        var entry = schedule.Find(s => s.scheduleBlock == blockName);
        if (entry == null) return;

        if (entry.destination != null && _brain != null)
            _brain.MoveTo(entry.destination.position);

        if (_brain != null)
            _brain.SetState(entry.stateOverride);
    }
}
