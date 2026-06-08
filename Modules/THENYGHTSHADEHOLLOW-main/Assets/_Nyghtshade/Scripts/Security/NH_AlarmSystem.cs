using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_AlarmSystem : MonoBehaviour
{
    public static NH_AlarmSystem Instance { get; private set; }

    public bool isActive;
    public string lastTriggerReason;
    public AudioClip alarmClip;

    public event Action<string> OnAlarmTriggered;
    public event Action OnAlarmCleared;

    private AudioSource _audio;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        _audio = GetComponent<AudioSource>();
    }

    public void TriggerAlarm(string reason)
    {
        isActive = true;
        lastTriggerReason = reason;
        OnAlarmTriggered?.Invoke(reason);

        if (_audio != null && alarmClip != null)
        {
            _audio.clip = alarmClip;
            _audio.loop = true;
            _audio.Play();
        }

        NH_LockdownManager.Instance?.ActivateLockdown(reason);
    }

    public void ClearAlarm()
    {
        isActive = false;
        if (_audio != null) _audio.Stop();
        OnAlarmCleared?.Invoke();
    }
}
