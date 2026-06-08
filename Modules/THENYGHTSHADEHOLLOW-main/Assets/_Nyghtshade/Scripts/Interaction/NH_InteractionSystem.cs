using UnityEngine;

public class NH_InteractionSystem : MonoBehaviour
{
    public float interactRange = 2.5f;
    public LayerMask interactableMask = ~0;
    public KeyCode interactKey = KeyCode.E;

    private NH_PlayerStats _stats;
    private NH_IInteractable _currentTarget;
    private NH_InteractionPromptUI _promptUI;

    void Awake()
    {
        _stats = GetComponent<NH_PlayerStats>();
    }

    void Start()
    {
        _promptUI = FindFirstObjectByType<NH_InteractionPromptUI>();
    }

    void Update()
    {
        DetectInteractable();

        if (_currentTarget != null && Input.GetKeyDown(interactKey))
        {
            if (_currentTarget.CanInteract(_stats))
                _currentTarget.Interact(_stats);
        }
    }

    void DetectInteractable()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableMask))
        {
            NH_IInteractable interactable = hit.collider.GetComponentInParent<NH_IInteractable>();
            if (interactable != null && interactable.CanInteract(_stats))
            {
                _currentTarget = interactable;
                _promptUI?.Show(interactable.InteractVerb, interactable.DisplayName);
                return;
            }
        }

        _currentTarget = null;
        _promptUI?.Hide();
    }
}
