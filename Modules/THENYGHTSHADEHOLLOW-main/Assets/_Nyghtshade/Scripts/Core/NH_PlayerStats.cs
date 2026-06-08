using UnityEngine;
using System;

public class NH_PlayerStats : MonoBehaviour
{
    [Header("Vitals")]
    [Range(0f, 100f)] public float health = 100f;
    [Range(0f, 100f)] public float stamina = 100f;
    [Range(0f, 100f)] public float stress = 0f;
    [Range(0f, 100f)] public float suspicionExposure = 0f;

    [Header("Regen")]
    public float staminaRegenRate = 10f;
    public float stressDecayRate = 2f;

    [Header("Player Path Flags")]
    public bool pathFloatThrough;
    public bool pathEscapeArtist;
    public bool pathYardKing;
    public bool pathInvestigator;
    public bool pathRedeemer;
    public bool pathCorruptor;
    public bool pathGRAVE;

    public float Health => health;
    public float Stamina => stamina;
    public float Stress => stress;
    public float SuspicionExposure => suspicionExposure;
    public bool IsIncapacitated => health <= 0f;
    public string CurrentZone { get; set; } = "Unknown";
    public string CurrentScheduleState { get; set; } = "Unknown";

    public event Action<float> OnHealthChanged;
    public event Action<float> OnStressChanged;
    public event Action<float> OnSuspicionChanged;
    public event Action OnIncapacitated;

    void Update()
    {
        RegenStamina();
        DecayStress();
    }

    void RegenStamina()
    {
        if (stamina < 100f)
            stamina = Mathf.Min(100f, stamina + staminaRegenRate * Time.deltaTime);
    }

    void DecayStress()
    {
        if (stress > 0f)
            stress = Mathf.Max(0f, stress - stressDecayRate * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        health = Mathf.Max(0f, health - amount);
        OnHealthChanged?.Invoke(health);
        if (health <= 0f) OnIncapacitated?.Invoke();
    }

    public void Heal(float amount)
    {
        health = Mathf.Min(100f, health + amount);
        OnHealthChanged?.Invoke(health);
    }

    public void DrainStamina(float amount)
    {
        stamina = Mathf.Max(0f, stamina - amount);
    }

    public void AddStress(float amount)
    {
        stress = Mathf.Min(100f, stress + amount);
        OnStressChanged?.Invoke(stress);
    }

    public void AddSuspicion(float amount)
    {
        suspicionExposure = Mathf.Min(100f, suspicionExposure + amount);
        OnSuspicionChanged?.Invoke(suspicionExposure);
    }

    public void ReduceSuspicion(float amount)
    {
        suspicionExposure = Mathf.Max(0f, suspicionExposure - amount);
        OnSuspicionChanged?.Invoke(suspicionExposure);
    }
}
