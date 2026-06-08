using UnityEngine;

public class NH_SearchSystem : MonoBehaviour
{
    public static NH_SearchSystem Instance { get; private set; }

    [Range(0f, 1f)] public float searchThoroughness = 0.5f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public NH_SearchResult SearchPlayer(NH_PlayerStats player)
    {
        var inv = player.GetComponent<NH_PlayerInventory>();
        if (inv == null) return NH_SearchResult.Clear;

        float heat = inv.GetTotalHeat();
        float detectionChance = searchThoroughness * (heat / 100f);

        if (Random.value > detectionChance)
            return NH_SearchResult.Clear;

        var contraband = inv.GetContrabandItems();
        if (contraband.Count == 0) return NH_SearchResult.Clear;

        inv.ConfiscateAll();
        NH_GuardSuspicionSystem.Instance?.AddSuspicion(30f, "items_confiscated");
        player.AddSuspicion(20f);

        return NH_SearchResult.ItemsFound;
    }
}

public enum NH_SearchResult { Clear, ItemsFound, EvidenceFound, WeaponFound }
