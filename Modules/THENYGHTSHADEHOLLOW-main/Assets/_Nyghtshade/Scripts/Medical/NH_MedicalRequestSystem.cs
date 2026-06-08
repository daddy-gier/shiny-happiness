using UnityEngine;
using System;

public class NH_MedicalRequestSystem : MonoBehaviour
{
    public static NH_MedicalRequestSystem Instance { get; private set; }

    [Header("Medical State")]
    public int pendingRequests;
    public float averageWaitTime = 30f;
    public bool isOverloaded;

    public event Action<int> OnRequestsChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SubmitRequest(NH_PlayerStats player)
    {
        pendingRequests++;
        OnRequestsChanged?.Invoke(pendingRequests);

        if (pendingRequests > 5)
        {
            isOverloaded = true;
            NH_MedicalNeglectMeter.Instance?.AddNeglect(5f);
        }
    }

    public void TreatPlayer(NH_PlayerStats player, float healAmount)
    {
        if (pendingRequests > 0) pendingRequests--;
        player.Heal(healAmount);
        player.AddStress(-20f);
        OnRequestsChanged?.Invoke(pendingRequests);
    }
}
