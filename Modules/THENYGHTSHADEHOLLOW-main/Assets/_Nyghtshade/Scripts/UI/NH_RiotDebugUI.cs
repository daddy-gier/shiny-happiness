using UnityEngine;
using TMPro;

public class NH_RiotDebugUI : MonoBehaviour
{
    public TMP_Text riotInfoText;

    void Update()
    {
        if (riotInfoText == null || NH_RiotManager.Instance == null) return;

        var rm = NH_RiotManager.Instance;
        riotInfoText.text = rm.isActive
            ? $"RIOT ACTIVE\nPhase: {rm.currentPhase}\nThreat: {rm.riotThreat:F0}"
            : $"Riot Threat: {rm.riotThreat:F0}\n(No Active Riot)";
    }
}
