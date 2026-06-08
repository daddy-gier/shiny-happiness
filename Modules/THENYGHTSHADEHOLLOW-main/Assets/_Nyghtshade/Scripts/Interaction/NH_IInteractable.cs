public interface NH_IInteractable
{
    string DisplayName { get; }
    string InteractVerb { get; }
    bool CanInteract(NH_PlayerStats player);
    void Interact(NH_PlayerStats player);
}
