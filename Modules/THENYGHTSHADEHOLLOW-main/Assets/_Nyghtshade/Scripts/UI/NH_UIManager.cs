using UnityEngine;

public class NH_UIManager : MonoBehaviour
{
    public static NH_UIManager Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject hudRoot;
    public GameObject inventoryRoot;
    public GameObject questJournalRoot;
    public GameObject factionRoot;
    public GameObject scheduleRoot;
    public GameObject lockdownWarningRoot;
    public GameObject riotDebugRoot;
    public GameObject debugOverlayRoot;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        SubscribeEvents();
        ShowHUD(true);
    }

    void SubscribeEvents()
    {
        if (NH_LockdownManager.Instance != null)
        {
            NH_LockdownManager.Instance.OnLockdownStarted += _ => ShowLockdownWarning(true);
            NH_LockdownManager.Instance.OnLockdownEnded += () => ShowLockdownWarning(false);
        }
        if (NH_RiotManager.Instance != null)
            NH_RiotManager.Instance.OnRiotStarted += () => ShowRiotDebug(true);
    }

    public void ShowHUD(bool show) => SetPanel(hudRoot, show);
    public void ShowInventory(bool show) => SetPanel(inventoryRoot, show);
    public void ShowQuestJournal(bool show) => SetPanel(questJournalRoot, show);
    public void ShowFactionUI(bool show) => SetPanel(factionRoot, show);
    public void ShowScheduleUI(bool show) => SetPanel(scheduleRoot, show);
    public void ShowLockdownWarning(bool show) => SetPanel(lockdownWarningRoot, show);
    public void ShowRiotDebug(bool show) => SetPanel(riotDebugRoot, show);
    public void ToggleDebugOverlay() => SetPanel(debugOverlayRoot, !debugOverlayRoot?.activeSelf ?? false);

    void SetPanel(GameObject panel, bool show)
    {
        if (panel != null) panel.SetActive(show);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) ShowInventory(inventoryRoot != null && !inventoryRoot.activeSelf);
        if (Input.GetKeyDown(KeyCode.J)) ShowQuestJournal(questJournalRoot != null && !questJournalRoot.activeSelf);
        if (Input.GetKeyDown(KeyCode.F3)) ToggleDebugOverlay();
    }
}
