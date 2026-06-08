using UnityEngine;

public class NH_ChapelSystem : MonoBehaviour
{
    public static NH_ChapelSystem Instance { get; private set; }

    [Header("Chapel State")]
    public bool isSafeZone = true;
    public bool servicesActive;
    public float spiritualInfluence;
    public bool GRAVEMarrowChurchConnected;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void StartServices()
    {
        servicesActive = true;
        spiritualInfluence = Mathf.Min(100f, spiritualInfluence + 10f);
    }

    public void EndServices() => servicesActive = false;

    public void TriggerGRAVEConnection()
    {
        GRAVEMarrowChurchConnected = true;
        NH_StoryFlagManager.Instance.SetFlag("chapel_grave_connected");
        NH_GRAVEManager.Instance?.RevealConnection("chapel");
    }

    public void PlayerEnter(NH_PlayerStats player)
    {
        if (isSafeZone)
            player.AddStress(-15f);
    }
}
