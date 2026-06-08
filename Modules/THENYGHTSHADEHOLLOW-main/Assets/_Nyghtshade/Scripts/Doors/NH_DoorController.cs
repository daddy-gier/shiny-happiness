using UnityEngine;

public class NH_DoorController : NH_InteractableBase
{
    [Header("Door Config")]
    public NH_DoorState doorState = NH_DoorState.Closed;
    public NH_DoorLockData lockData;
    public bool isSliding;
    public Vector3 openOffset = new Vector3(0f, 0f, 2f);
    public float animSpeed = 2f;

    [Header("Audio")]
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip lockedSound;
    public AudioClip alarmSound;

    private Vector3 _closedPos;
    private Vector3 _openPos;
    private bool _animating;
    private float _animT;
    private AudioSource _audio;

    void Start()
    {
        _closedPos = transform.position;
        _openPos = transform.position + openOffset;
        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_animating) AnimateDoor();
    }

    public override string InteractVerb => doorState == NH_DoorState.Open ? "Close" : "Open";
    public override string DisplayName => displayName;

    public override bool CanInteract(NH_PlayerStats player)
    {
        if (doorState == NH_DoorState.LockdownSealed || doorState == NH_DoorState.Jammed)
            return false;
        return true;
    }

    public override void Interact(NH_PlayerStats player)
    {
        if (doorState == NH_DoorState.Open) { Close(); return; }

        if (!CanUnlock(player))
        {
            PlaySound(lockedSound);
            if (lockData != null && lockData.triggerAlarmOnForce)
                NH_AlarmSystem.Instance?.TriggerAlarm("forced_door");
            return;
        }

        Open();
    }

    bool CanUnlock(NH_PlayerStats player)
    {
        if (doorState == NH_DoorState.Closed) return true;
        if (lockData == null) return true;

        if (!string.IsNullOrEmpty(lockData.requiredItemId))
        {
            var inv = player?.GetComponent<NH_PlayerInventory>();
            if (inv == null || !inv.HasItem(lockData.requiredItemId)) return false;
        }
        if (!string.IsNullOrEmpty(lockData.requiredQuestFlag))
        {
            if (!NH_StoryFlagManager.Instance.HasFlag(lockData.requiredQuestFlag)) return false;
        }
        return true;
    }

    public void Open()
    {
        doorState = NH_DoorState.Open;
        _animating = true;
        _animT = 0f;
        PlaySound(openSound);
    }

    public void Close()
    {
        doorState = NH_DoorState.Closed;
        _animating = true;
        _animT = 0f;
        PlaySound(closeSound);
    }

    public void LockdownSeal()
    {
        doorState = NH_DoorState.LockdownSealed;
        if (doorState == NH_DoorState.Open) { Close(); }
    }

    public void Release()
    {
        if (doorState == NH_DoorState.LockdownSealed)
            doorState = NH_DoorState.Closed;
    }

    void AnimateDoor()
    {
        _animT += Time.deltaTime * animSpeed;
        if (isSliding)
        {
            Vector3 target = doorState == NH_DoorState.Open ? _openPos : _closedPos;
            transform.position = Vector3.Lerp(transform.position, target, _animT);
        }
        else
        {
            float targetAngle = doorState == NH_DoorState.Open ? 90f : 0f;
            Vector3 current = transform.localEulerAngles;
            current.y = Mathf.LerpAngle(current.y, targetAngle, _animT);
            transform.localEulerAngles = current;
        }
        if (_animT >= 1f) _animating = false;
    }

    void PlaySound(AudioClip clip)
    {
        if (_audio != null && clip != null) _audio.PlayOneShot(clip);
    }
}
