using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_PrisonScheduleManager : MonoBehaviour
{
    public static NH_PrisonScheduleManager Instance { get; private set; }

    [Header("Schedule")]
    public List<NH_ScheduleBlock> schedule = new List<NH_ScheduleBlock>();
    public float gameTimeScale = 60f;

    private int _currentIndex;
    private float _elapsed;
    private int _currentDay = 1;

    public NH_ScheduleBlock CurrentBlock => schedule.Count > 0 ? schedule[_currentIndex] : null;
    public int CurrentDay => _currentDay;
    public string CurrentBlockName => CurrentBlock?.displayName ?? "Unknown";

    public event Action<string> OnScheduleBlockChanged;
    public event Action<int> OnNewDay;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (schedule.Count == 0) BuildDefaultSchedule();
    }

    void Update()
    {
        if (schedule.Count == 0 || NH_LockdownManager.Instance?.isLockdownActive == true) return;

        _elapsed += Time.deltaTime * gameTimeScale;
        float blockDurationSec = CurrentBlock.durationMinutes * 60f / gameTimeScale;

        if (_elapsed >= blockDurationSec)
        {
            _elapsed = 0f;
            AdvanceBlock();
        }

        var player = FindFirstObjectByType<NH_PlayerStats>();
        if (player != null) player.CurrentScheduleState = CurrentBlockName;
    }

    void AdvanceBlock()
    {
        _currentIndex = (_currentIndex + 1) % schedule.Count;
        if (_currentIndex == 0) { _currentDay++; OnNewDay?.Invoke(_currentDay); }
        OnScheduleBlockChanged?.Invoke(CurrentBlock.displayName);
    }

    public bool IsCurrentlyAllowed(string zone)
    {
        var block = CurrentBlock;
        if (block == null) return true;
        return zone switch
        {
            "Yard" => block.allowYardAccess,
            "Cafeteria" => block.allowCafeteriaAccess,
            "Infirmary" => block.allowMedicalAccess,
            "AdminControl" => block.allowAdminAccess,
            _ => !block.isLockIn
        };
    }

    void BuildDefaultSchedule()
    {
        schedule.Add(new NH_ScheduleBlock { state = NH_ScheduleState.WakeUp, displayName = "Wake Up", durationMinutes = 30 });
        schedule.Add(new NH_ScheduleBlock { state = NH_ScheduleState.Count, displayName = "Count", durationMinutes = 20 });
        schedule.Add(new NH_ScheduleBlock { state = NH_ScheduleState.Breakfast, displayName = "Breakfast", durationMinutes = 45, allowCafeteriaAccess = true });
        schedule.Add(new NH_ScheduleBlock { state = NH_ScheduleState.WorkPrograms, displayName = "Work / Programs", durationMinutes = 120 });
        schedule.Add(new NH_ScheduleBlock { state = NH_ScheduleState.Yard, displayName = "Yard Time", durationMinutes = 60, allowYardAccess = true });
        schedule.Add(new NH_ScheduleBlock { state = NH_ScheduleState.Lunch, displayName = "Lunch", durationMinutes = 45, allowCafeteriaAccess = true });
        schedule.Add(new NH_ScheduleBlock { state = NH_ScheduleState.MedicalPrograms, displayName = "Medical / Programs", durationMinutes = 90, allowMedicalAccess = true });
        schedule.Add(new NH_ScheduleBlock { state = NH_ScheduleState.Recreation, displayName = "Recreation", durationMinutes = 90 });
        schedule.Add(new NH_ScheduleBlock { state = NH_ScheduleState.Dinner, displayName = "Dinner", durationMinutes = 45, allowCafeteriaAccess = true });
        schedule.Add(new NH_ScheduleBlock { state = NH_ScheduleState.EveningMovement, displayName = "Evening Movement", durationMinutes = 30 });
        schedule.Add(new NH_ScheduleBlock { state = NH_ScheduleState.LockIn, displayName = "Lock In", durationMinutes = 60, isLockIn = true });
        schedule.Add(new NH_ScheduleBlock { state = NH_ScheduleState.Night, displayName = "Night", durationMinutes = 240, isLockIn = true });
        schedule.Add(new NH_ScheduleBlock { state = NH_ScheduleState.DeepNight, displayName = "Deep Night", durationMinutes = 120, isLockIn = true });
    }
}
