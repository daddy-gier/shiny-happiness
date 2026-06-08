using UnityEngine;

[CreateAssetMenu(fileName = "DoorLock", menuName = "Nyghtshade/Door Lock Data")]
public class NH_DoorLockData : ScriptableObject
{
    public NH_KeycardAccessLevel requiredAccess;
    public string requiredItemId;
    public string requiredQuestFlag;
    public string requiredFactionId;
    public bool requiresScheduleBlock;
    public string allowedScheduleBlock;
    public bool triggerAlarmOnForce;
}
