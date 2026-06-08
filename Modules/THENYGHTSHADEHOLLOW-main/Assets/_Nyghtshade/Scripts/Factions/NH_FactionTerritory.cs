using UnityEngine;

public class NH_FactionTerritory : MonoBehaviour
{
    public string factionId;
    public string zoneName;
    public bool isContestedZone;

    public void ClaimTerritory(string newFactionId)
    {
        factionId = newFactionId;
        NH_AreaControlManager.Instance?.SetZoneControl(zoneName, NH_AreaControlState.FactionControlled, factionId);
    }

    public void LoseTerritory()
    {
        factionId = string.Empty;
        NH_AreaControlManager.Instance?.SetZoneControl(zoneName, NH_AreaControlState.Neutral);
    }
}
