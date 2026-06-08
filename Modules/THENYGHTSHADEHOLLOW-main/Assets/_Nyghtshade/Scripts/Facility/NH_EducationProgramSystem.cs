using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_EducationProgramSystem : MonoBehaviour
{
    public static NH_EducationProgramSystem Instance { get; private set; }

    [Serializable]
    public class Program
    {
        public string programId;
        public string displayName;
        public bool isEnrolled;
        public float progressPercent;
        public float trustRequired;
        public bool isActive = true;
    }

    public List<Program> programs = new List<Program>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool Enroll(string programId)
    {
        var p = programs.Find(prog => prog.programId == programId);
        if (p == null || !p.isActive) return false;
        p.isEnrolled = true;
        return true;
    }

    public void AdvanceProgress(string programId, float amount)
    {
        var p = programs.Find(prog => prog.programId == programId);
        if (p == null) return;
        p.progressPercent = Mathf.Min(100f, p.progressPercent + amount);
        if (p.progressPercent >= 100f)
            NH_StoryFlagManager.Instance.SetFlag($"program_complete_{programId}");
    }

    public void ShutdownProgram(string programId)
    {
        var p = programs.Find(prog => prog.programId == programId);
        if (p != null) p.isActive = false;
    }
}
