using UnityEngine;
using TMPro;

public class NH_ScheduleUI : MonoBehaviour
{
    public TMP_Text blockLabel;
    public TMP_Text dayLabel;

    void OnEnable()
    {
        if (NH_PrisonScheduleManager.Instance != null)
            NH_PrisonScheduleManager.Instance.OnScheduleBlockChanged += OnBlockChanged;
        Refresh();
    }

    void OnDisable()
    {
        if (NH_PrisonScheduleManager.Instance != null)
            NH_PrisonScheduleManager.Instance.OnScheduleBlockChanged -= OnBlockChanged;
    }

    void OnBlockChanged(string blockName) => Refresh();

    void Refresh()
    {
        var sched = NH_PrisonScheduleManager.Instance;
        if (sched == null) return;
        if (blockLabel) blockLabel.text = sched.CurrentBlockName;
        if (dayLabel) dayLabel.text = $"Day {sched.CurrentDay}";
    }
}
