using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Mouse Look")]
    public float mouseSensitivity = 0.15f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    [Header("Zoom")]
    public float defaultDistance = 5f;
    public float minDistance = 2f;
    public float maxDistance = 8f;
    public float zoomSpeed = 1f;
    public float zoomSmoothSpeed = 10f;

    [Header("Wall Collision")]
    public LayerMask collisionLayers;
    public float collisionRadius = 0.2f;
    public float collisionOffset = 0.1f;

    private float yaw;
    private float pitch;

    private float targetDistance;
    private float currentDistance;

    void Start()
    {
        yaw = player.eulerAngles.y;

        pitch = transform.localEulerAngles.x;

        if (pitch > 180f)
            pitch -= 360f;

        targetDistance = defaultDistance;
        currentDistance = defaultDistance;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
        Zoom();
    }

    void LateUpdate()
    {
        HandleCameraCollision();

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 cameraPosition =
            transform.position - transform.forward * currentDistance;

        cameraTransform.position = cameraPosition;
    }

    void Look()
    {
        if (Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    void Zoom()
    {
        if (Mouse.current == null)
            return;

        float scroll = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetDistance -= scroll * zoomSpeed * 0.01f;

            targetDistance = Mathf.Clamp(
                targetDistance,
                minDistance,
                maxDistance
            );
        }
    }

    void HandleCameraCollision()
    {
        Vector3 origin = transform.position;
        Vector3 direction = -transform.forward;

        float safeDistance = targetDistance;

        if (Physics.SphereCast(
            origin,
            collisionRadius,
            direction,
            out RaycastHit hit,
            targetDistance,
            collisionLayers,
            QueryTriggerInteraction.Ignore))
        {
            safeDistance = hit.distance - collisionOffset;

            safeDistance = Mathf.Clamp(
                safeDistance,
                minDistance,
                targetDistance
            );
        }

        currentDistance = Mathf.Lerp(
            currentDistance,
            safeDistance,
            zoomSmoothSpeed * Time.deltaTime
        );
    }
}