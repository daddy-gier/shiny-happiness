using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_GRAVEManager : MonoBehaviour
{
    public static NH_GRAVEManager Instance { get; private set; }

    [Header("GRAVE State")]
    public bool tunnelAccessUnlocked;
    public bool horrorAwakened;
    [Range(0f, 100f)] public float mysteryDepth;

    private List<string> _discoveredClues = new List<string>();
    private List<string> _connections = new List<string>();

    private NH_TunnelDiscoveryMap _tunnelMap;

    public event Action<string> OnClueDiscovered;
    public event Action<string> OnConnectionRevealed;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        _tunnelMap = GetComponent<NH_TunnelDiscoveryMap>();
    }

    public void DiscoverClue(string clueId)
    {
        if (_discoveredClues.Contains(clueId)) return;
        _discoveredClues.Add(clueId);
        mysteryDepth = Mathf.Min(100f, mysteryDepth + 10f);
        NH_StoryFlagManager.Instance.SetFlag($"grave_clue_{clueId}");
        OnClueDiscovered?.Invoke(clueId);

        if (mysteryDepth >= 50f && !horrorAwakened)
            AwakenHorror();
    }

    public void RevealConnection(string source)
    {
        if (_connections.Contains(source)) return;
        _connections.Add(source);
        mysteryDepth = Mathf.Min(100f, mysteryDepth + 5f);
        OnConnectionRevealed?.Invoke(source);
        Debug.Log($"[GRAVE] Connection revealed from: {source}");
    }

    public void EnterTunnel(NH_PlayerStats player, string sectionId)
    {
        _tunnelMap?.DiscoverSection(sectionId);
        NH_StoryFlagManager.Instance.SetFlag("entered_grave_tunnel");
        player.AddStress(20f);
    }

    void AwakenHorror()
    {
        horrorAwakened = true;
        NH_StoryFlagManager.Instance.SetFlag("grave_horror_awakened");
        Debug.Log("[GRAVE] The horror awakens.");
    }

    public List<string> GetDiscoveredClues() => new List<string>(_discoveredClues);
}
