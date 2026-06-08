using System;
using System.Collections;
using UnityEngine;

public class NH_LockdownManager : MonoBehaviour
{
    public static NH_LockdownManager Instance { get; private set; }

    public bool isLockdownActive;
    public float lockdownSeverity;
    public string lockdownReason;

    public event Action<string> OnLockdownStarted;
    public event Action OnLockdownEnded;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void ActivateLockdown(string reason, float severity = 50f)
    {
        if (isLockdownActive) return;

        isLockdownActive = true;
        lockdownSeverity = severity;
        lockdownReason = reason;

        SealAllDoors();
        SetAllNPCsToLockdownPosition();

        OnLockdownStarted?.Invoke(reason);
        NH_ContrabandSystem.Instance?.AddGlobalHeat(20f);

        Debug.Log($"[NH] LOCKDOWN ACTIVE: {reason}");
    }

    public void EndLockdown()
    {
        isLockdownActive = false;
        lockdownSeverity = 0f;
        ReleaseDoors();
        OnLockdownEnded?.Invoke();
    }

    void SealAllDoors()
    {
        foreach (var door in FindObjectsByType<NH_DoorController>(FindObjectsSortMode.None))
            door.LockdownSeal();
    }

    void ReleaseDoors()
    {
        foreach (var door in FindObjectsByType<NH_DoorController>(FindObjectsSortMode.None))
            door.Release();
    }

    void SetAllNPCsToLockdownPosition()
    {
        foreach (var brain in FindObjectsByType<NH_NPCBrain>(FindObjectsSortMode.None))
            brain.SetState(NH_NPCState.LockdownPosition);
    }
}
