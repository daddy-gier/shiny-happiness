using UnityEngine;

[System.Serializable]
public class NH_ScheduleBlock
{
    public NH_ScheduleState state;
    public string displayName;
    public float durationMinutes = 60f;
    public bool allowYardAccess;
    public bool allowCafeteriaAccess;
    public bool allowMedicalAccess;
    public bool allowAdminAccess;
    public bool isLockIn;
    public float suspicionIfViolated = 15f;
}
