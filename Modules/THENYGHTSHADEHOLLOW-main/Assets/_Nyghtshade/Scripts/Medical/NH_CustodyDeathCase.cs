using UnityEngine;

[CreateAssetMenu(fileName = "DeathCase", menuName = "Nyghtshade/Custody Death Case")]
public class NH_CustodyDeathCase : ScriptableObject
{
    public string caseId;
    public string victimName;
    [TextArea] public string officialCause;
    [TextArea] public string trueCircumstances;
    public bool isCoveredUp;
    public bool evidenceDiscovered;
    public string relatedQuestId;
    public bool linksToGRAVE;
    public string[] linkedEvidenceIds;
}
