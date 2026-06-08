using UnityEngine;
using TMPro;

public class NH_FactionUI : MonoBehaviour
{
    public TMP_Text factionText;

    void OnEnable() => Refresh();

    void Refresh()
    {
        if (factionText == null || NH_FactionManager.Instance == null) return;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("=== FACTION STANDINGS ===\n");

        foreach (var def in NH_FactionManager.Instance.factionDefinitions)
        {
            var s = NH_FactionManager.Instance.GetStanding(def.factionId);
            sb.AppendLine($"{def.displayName}: Respect {s.respect:+0;-0}  Fear {s.fear:F0}  Trust {s.trust:F0}");
        }

        factionText.text = sb.ToString();
    }
}
