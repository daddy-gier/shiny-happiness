using UnityEngine;

public class NH_RiotAreaObjective : MonoBehaviour
{
    public string zoneName;
    public NH_AreaControlState targetControl;
    public string ownerFactionId;
    public bool isAchieved;

    public void TryClaim(string factionId)
    {
        if (isAchieved) return;
        ownerFactionId = factionId;
        isAchieved = true;
        NH_AreaControlManager.Instance?.SetZoneControl(zoneName, targetControl, factionId);
        NH_FactionManager.Instance?.ModifyRespect(factionId, 10f);
        Debug.Log($"[NH Riot] {factionId} claimed {zoneName}.");
    }
}
