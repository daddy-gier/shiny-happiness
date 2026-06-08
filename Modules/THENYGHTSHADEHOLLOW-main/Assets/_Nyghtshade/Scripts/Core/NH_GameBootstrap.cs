using UnityEngine;

public class NH_GameBootstrap : MonoBehaviour
{
    public static NH_GameBootstrap Instance { get; private set; }

    [Header("Prefabs — assign in Inspector or let bootstrap find them")]
    public GameObject playerPrefab;
    public GameObject cameraPrefab;

    [Header("Settings")]
    public bool spawnPlayerOnStart = true;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        EnforceSingleAudioListener();
        EnforceSingleMainCamera();
        if (spawnPlayerOnStart) SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (FindFirstObjectByType<NH_PlayerController>() != null) return;

        NH_PlayerSpawnPoint spawn = FindFirstObjectByType<NH_PlayerSpawnPoint>();
        Vector3 pos = spawn != null ? spawn.Position : Vector3.zero;
        Quaternion rot = spawn != null ? spawn.Rotation : Quaternion.identity;

        if (playerPrefab != null)
            Instantiate(playerPrefab, pos, rot);
    }

    void EnforceSingleAudioListener()
    {
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        for (int i = 1; i < listeners.Length; i++)
            listeners[i].enabled = false;
    }

    void EnforceSingleMainCamera()
    {
        Camera[] cams = FindObjectsByType<Camera>(FindObjectsSortMode.None);
        bool foundMain = false;
        foreach (var cam in cams)
        {
            if (cam.CompareTag("MainCamera"))
            {
                if (foundMain) cam.enabled = false;
                else foundMain = true;
            }
        }
    }
}
