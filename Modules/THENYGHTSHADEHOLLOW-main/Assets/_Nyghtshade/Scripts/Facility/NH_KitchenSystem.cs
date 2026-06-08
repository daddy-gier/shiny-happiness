using System;
using UnityEngine;

public class NH_KitchenSystem : MonoBehaviour
{
    public static NH_KitchenSystem Instance { get; private set; }

    [Header("Kitchen State")]
    [Range(0f, 100f)] public float foodQuality = 50f;
    public bool toolCountComplete = true;
    public string controllingFactionId;
    public int missingToolCount;

    public event Action OnMissingToolLockdown;
    public event Action<float> OnFoodQualityChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SetFoodQuality(float quality)
    {
        foodQuality = Mathf.Clamp(quality, 0f, 100f);
        OnFoodQualityChanged?.Invoke(foodQuality);

        if (foodQuality < 20f)
            NH_RiotManager.Instance?.AddRiotTrigger("bad_food");
    }

    public void ReportMissingTool()
    {
        missingToolCount++;
        toolCountComplete = false;
        OnMissingToolLockdown?.Invoke();
        NH_LockdownManager.Instance?.ActivateLockdown("missing_kitchen_tool", 40f);
    }

    public void ToolsAccountedFor()
    {
        missingToolCount = 0;
        toolCountComplete = true;
    }
}
