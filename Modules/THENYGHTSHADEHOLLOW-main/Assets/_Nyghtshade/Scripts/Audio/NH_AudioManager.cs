using System.Collections.Generic;
using UnityEngine;

public class NH_AudioManager : MonoBehaviour
{
    public static NH_AudioManager Instance { get; private set; }

    [System.Serializable]
    public class AudioEntry
    {
        public string audioId;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop;
    }

    [Header("Audio Library")]
    public List<AudioEntry> audioLibrary = new List<AudioEntry>();

    private Dictionary<string, AudioEntry> _library = new Dictionary<string, AudioEntry>();
    private Dictionary<string, AudioSource> _activeSources = new Dictionary<string, AudioSource>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        foreach (var entry in audioLibrary)
            _library[entry.audioId] = entry;
    }

    public void Play(string audioId)
    {
        if (!_library.TryGetValue(audioId, out var entry) || entry.clip == null) return;

        if (!_activeSources.TryGetValue(audioId, out var source))
        {
            source = gameObject.AddComponent<AudioSource>();
            _activeSources[audioId] = source;
        }

        source.clip = entry.clip;
        source.volume = entry.volume;
        source.loop = entry.loop;
        source.Play();
    }

    public void Stop(string audioId)
    {
        if (_activeSources.TryGetValue(audioId, out var source))
            source.Stop();
    }

    public void PlayOneShot(string audioId)
    {
        if (!_library.TryGetValue(audioId, out var entry) || entry.clip == null) return;

        var source = gameObject.AddComponent<AudioSource>();
        source.PlayOneShot(entry.clip, entry.volume);
        Destroy(source, entry.clip.length + 0.1f);
    }
}
