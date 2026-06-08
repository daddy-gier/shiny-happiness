using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NH_PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3.5f;
    public float sprintSpeed = 6f;
    public float jumpHeight = 1.2f;
    public float gravity = -19.62f;
    public float rotationSmoothTime = 0.1f;

    [Header("References")]
    public Transform cameraTarget;

    private CharacterController _cc;
    private NH_PlayerStats _stats;
    private Vector3 _velocity;
    private float _rotationVelocity;
    private bool _grounded;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _stats = GetComponent<NH_PlayerStats>();
    }

    void Update()
    {
        if (_stats != null && _stats.IsIncapacitated) return;

        GroundCheck();
        HandleMovement();
        HandleGravity();
    }

    void GroundCheck()
    {
        _grounded = _cc.isGrounded;
        if (_grounded && _velocity.y < 0f)
            _velocity.y = -2f;
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        if (inputDir.magnitude < 0.1f) return;

        bool sprinting = Input.GetKey(KeyCode.LeftShift) && (_stats == null || _stats.Stamina > 0f);
        float speed = sprinting ? sprintSpeed : walkSpeed;

        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 camForward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;
            inputDir = (camForward * v + camRight * h).normalized;
        }

        float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationVelocity, rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        _cc.Move(inputDir * speed * Time.deltaTime);

        if (_stats != null && sprinting)
            _stats.DrainStamina(8f * Time.deltaTime);
    }

    void HandleGravity()
    {
        if (_grounded && Input.GetButtonDown("Jump") && (_stats == null || !_stats.IsIncapacitated))
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        _velocity.y += gravity * Time.deltaTime;
        _cc.Move(_velocity * Time.deltaTime);
    }

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        _cc.enabled = false;
        transform.SetPositionAndRotation(position, rotation);
        _cc.enabled = true;
    }
}
