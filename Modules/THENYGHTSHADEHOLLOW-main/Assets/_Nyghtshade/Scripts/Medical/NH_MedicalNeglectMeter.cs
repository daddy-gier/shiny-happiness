using System;
using UnityEngine;

public class NH_MedicalNeglectMeter : MonoBehaviour
{
    public static NH_MedicalNeglectMeter Instance { get; private set; }

    [Range(0f, 100f)] public float neglectLevel;
    [Range(0f, 100f)] public float triageSuspicion;

    public event Action<float> OnNeglectChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void AddNeglect(float amount)
    {
        neglectLevel = Mathf.Min(100f, neglectLevel + amount);
        OnNeglectChanged?.Invoke(neglectLevel);

        if (neglectLevel > 70f)
            NH_RiotManager.Instance?.AddRiotTrigger("medical_neglect_scandal");
    }

    public void AddTriageSuspicion(float amount)
    {
        triageSuspicion = Mathf.Min(100f, triageSuspicion + amount);
    }
}
