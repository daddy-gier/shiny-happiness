using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NH_DarknessZone : MonoBehaviour
{
    [Header("Darkness Settings")]
    public float stressPerSecond = 3f;
    public float fearAccumulation = 2f;
    public bool triggerVisions;
    public string visionFlag;

    private bool _playerInside;

    void Awake() => GetComponent<Collider>().isTrigger = true;

    void Update()
    {
        if (!_playerInside) return;

        var player = FindFirstObjectByType<NH_PlayerStats>();
        if (player == null) return;

        player.AddStress(stressPerSecond * Time.deltaTime);

        var survival = player.GetComponent<NH_SurvivalStatus>();
        survival?.ModifyFear(fearAccumulation * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _playerInside = true;
        if (triggerVisions && !string.IsNullOrEmpty(visionFlag))
            NH_StoryFlagManager.Instance.SetFlag(visionFlag);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _playerInside = false;
    }
}
