using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class NH_AmbienceZone : MonoBehaviour
{
    public string ambienceId;
    public AudioClip ambienceClip;
    [Range(0f, 1f)] public float targetVolume = 0.5f;
    public float fadeSpeed = 1f;

    private AudioSource _audio;
    private bool _playerInside;
    private float _currentVolume;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        _audio = GetComponent<AudioSource>();
        _audio.clip = ambienceClip;
        _audio.loop = true;
        _audio.volume = 0f;
        _audio.spatialBlend = 0f;
        if (ambienceClip != null) _audio.Play();
    }

    void Update()
    {
        float target = _playerInside ? targetVolume : 0f;
        _currentVolume = Mathf.MoveTowards(_currentVolume, target, fadeSpeed * Time.deltaTime);
        _audio.volume = _currentVolume;
    }

    void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) _playerInside = true; }
    void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) _playerInside = false; }
}
