using UnityEngine;
using TMPro;

public class NH_LockdownWarningUI : MonoBehaviour
{
    public TMP_Text warningText;
    public GameObject warningPanel;

    void OnEnable()
    {
        if (NH_LockdownManager.Instance != null)
        {
            NH_LockdownManager.Instance.OnLockdownStarted += Show;
            NH_LockdownManager.Instance.OnLockdownEnded += Hide;
        }
        if (warningPanel) warningPanel.SetActive(NH_LockdownManager.Instance?.isLockdownActive ?? false);
    }

    void OnDisable()
    {
        if (NH_LockdownManager.Instance != null)
        {
            NH_LockdownManager.Instance.OnLockdownStarted -= Show;
            NH_LockdownManager.Instance.OnLockdownEnded -= Hide;
        }
    }

    void Show(string reason)
    {
        if (warningPanel) warningPanel.SetActive(true);
        if (warningText) warningText.text = $"LOCKDOWN\n{reason.ToUpper()}";
    }

    void Hide()
    {
        if (warningPanel) warningPanel.SetActive(false);
    }
}
