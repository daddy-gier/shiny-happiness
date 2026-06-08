using UnityEngine;

public class NH_CameraBootstrap : MonoBehaviour
{
    [Header("Follow Settings")]
    public Vector3 offset = new Vector3(0f, 2.5f, -5f);
    public float followSmoothSpeed = 8f;
    public float lookSensitivity = 2f;
    public float pitchMin = -30f;
    public float pitchMax = 60f;

    private Transform _target;
    private float _yaw;
    private float _pitch;

    void Start()
    {
        NH_PlayerController pc = FindFirstObjectByType<NH_PlayerController>();
        if (pc != null) _target = pc.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (_target == null)
        {
            NH_PlayerController pc = FindFirstObjectByType<NH_PlayerController>();
            if (pc != null) _target = pc.transform;
            return;
        }

        _yaw += Input.GetAxis("Mouse X") * lookSensitivity;
        _pitch -= Input.GetAxis("Mouse Y") * lookSensitivity;
        _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);

        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3 desiredPos = _target.position + rotation * offset;

        transform.position = Vector3.Lerp(transform.position, desiredPos, followSmoothSpeed * Time.deltaTime);
        transform.LookAt(_target.position + Vector3.up * 1.5f);
    }
}
