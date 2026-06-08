using UnityEngine;

public abstract class NH_InteractableBase : MonoBehaviour, NH_IInteractable
{
    [Header("Interaction Config")]
    public string displayName = "Object";
    public string interactVerb = "Interact";
    public bool locked;
    public string lockedMessage = "Locked.";
    public float interactDistance = 2.5f;
    public string requiredItemId;
    public string requiredQuestFlag;
    public string requiredFaction;

    public virtual string DisplayName => displayName;
    public virtual string InteractVerb => interactVerb;

    public virtual bool CanInteract(NH_PlayerStats player)
    {
        if (locked) return false;
        if (!string.IsNullOrEmpty(requiredItemId))
        {
            var inv = player?.GetComponent<NH_PlayerInventory>();
            if (inv == null || !inv.HasItem(requiredItemId)) return false;
        }
        if (!string.IsNullOrEmpty(requiredQuestFlag))
        {
            if (!NH_StoryFlagManager.Instance.HasFlag(requiredQuestFlag)) return false;
        }
        return true;
    }

    public abstract void Interact(NH_PlayerStats player);

    void OnDrawGizmosSelected()
    {
        Gizmos.color = locked ? Color.red : Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}
