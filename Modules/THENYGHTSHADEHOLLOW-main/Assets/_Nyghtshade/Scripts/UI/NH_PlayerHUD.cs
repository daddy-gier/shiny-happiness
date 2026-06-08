using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NH_PlayerHUD : MonoBehaviour
{
    [Header("Sliders")]
    public Slider healthBar;
    public Slider staminaBar;
    public Slider stressBar;
    public Slider suspicionBar;

    [Header("Labels")]
    public TMP_Text zoneLabel;
    public TMP_Text scheduleLabel;
    public TMP_Text lockdownLabel;
    public TMP_Text heatLabel;

    private NH_PlayerStats _stats;

    void Start()
    {
        _stats = FindFirstObjectByType<NH_PlayerStats>();

        if (NH_LockdownManager.Instance != null)
        {
            NH_LockdownManager.Instance.OnLockdownStarted += r => SetLockdownLabel($"LOCKDOWN: {r}");
            NH_LockdownManager.Instance.OnLockdownEnded += () => SetLockdownLabel("");
        }
    }

    void Update()
    {
        if (_stats == null) { _stats = FindFirstObjectByType<NH_PlayerStats>(); return; }

        SetSlider(healthBar, _stats.Health);
        SetSlider(staminaBar, _stats.Stamina);
        SetSlider(stressBar, _stats.Stress);
        SetSlider(suspicionBar, _stats.SuspicionExposure);

        if (zoneLabel) zoneLabel.text = _stats.CurrentZone;
        if (scheduleLabel) scheduleLabel.text = _stats.CurrentScheduleState;

        float heat = NH_ContrabandSystem.Instance?.GetPlayerHeat() ?? 0f;
        if (heatLabel) heatLabel.text = heat > 0f ? $"HEAT: {heat:F0}" : "";
    }

    void SetSlider(Slider s, float val)
    {
        if (s != null) s.value = val / 100f;
    }

    void SetLockdownLabel(string text)
    {
        if (lockdownLabel) lockdownLabel.text = text;
    }
}
