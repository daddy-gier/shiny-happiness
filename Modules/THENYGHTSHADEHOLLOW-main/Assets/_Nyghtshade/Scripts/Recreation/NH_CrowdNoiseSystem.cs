using UnityEngine;

public class NH_CrowdNoiseSystem : MonoBehaviour
{
    public static NH_CrowdNoiseSystem Instance { get; private set; }

    [Range(0f, 100f)] public float noiseLevel;
    public float noiseMaskingForGuards = 20f;
    public AudioSource crowdAudioSource;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SetNoiseLevel(float level)
    {
        noiseLevel = Mathf.Clamp(level, 0f, 100f);
        if (crowdAudioSource != null)
            crowdAudioSource.volume = noiseLevel / 100f;

        if (noiseLevel > 80f)
            NH_GuardSuspicionSystem.Instance?.AddSuspicion(5f * Time.deltaTime, "crowd_noise");
    }

    public void PulseNoise(float amount, float duration)
    {
        SetNoiseLevel(noiseLevel + amount);
    }

    public bool IsMaskingPlayerActions() => noiseLevel > noiseMaskingForGuards;
}
