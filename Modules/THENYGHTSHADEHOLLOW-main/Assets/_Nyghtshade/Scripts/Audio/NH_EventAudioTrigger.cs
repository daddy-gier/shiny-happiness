using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NH_EventAudioTrigger : MonoBehaviour
{
    public string audioId;
    public bool oneShot = true;
    private bool _triggered;

    void Awake() => GetComponent<Collider>().isTrigger = true;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (oneShot && _triggered) return;
        _triggered = true;
        NH_AudioManager.Instance?.PlayOneShot(audioId);
    }
}
