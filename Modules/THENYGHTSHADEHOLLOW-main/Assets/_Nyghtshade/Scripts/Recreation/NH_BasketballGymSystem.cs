using System;
using UnityEngine;

public class NH_BasketballGymSystem : MonoBehaviour
{
    public static NH_BasketballGymSystem Instance { get; private set; }

    [Header("Gym State")]
    public bool isOpen;
    public string controllingFactionId;
    public float tensionLevel;
    public bool fightEscalated;

    public event Action OnFightEscalated;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void OpenGym()
    {
        isOpen = true;
        NH_CrowdNoiseSystem.Instance?.SetNoiseLevel(40f);
    }

    public void CloseGym()
    {
        isOpen = false;
        NH_CrowdNoiseSystem.Instance?.SetNoiseLevel(0f);
    }

    public void AddTension(float amount)
    {
        tensionLevel = Mathf.Min(100f, tensionLevel + amount);
        if (tensionLevel > 80f && !fightEscalated) EscalateFight();
    }

    void EscalateFight()
    {
        fightEscalated = true;
        NH_CrowdNoiseSystem.Instance?.PulseNoise(50f, 10f);
        NH_GuardSuspicionSystem.Instance?.AddSuspicion(20f, "gym_fight");
        OnFightEscalated?.Invoke();
    }

    public void PlayerEnter(NH_PlayerStats player)
    {
        player.AddStress(-5f);
        if (fightEscalated) player.AddSuspicion(10f);
    }
}
