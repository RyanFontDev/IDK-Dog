using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float rotationSpeed = 10f;

    private MovingPlatform currentPlatform;
    private RotatingPlatform currentRotatingPlatform;

    [Header("Jumping")]
    public float jumpHeight = 2f;
    public float coyoteTime = 0.15f;

    private float coyoteTimeCounter;

    [Header("Double Jump")]
    public bool hasDoubleJump = false;
    public float doubleJumpHeight = 2f;

    private bool doubleJumpAvailable;


    [Header("Gravity")]
    public float gravity = -20f;
    public float groundedForce = -2f;

    [Header("Camera")]
    public Transform cameraTransform;

    private CharacterController controller;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleGravity();
        HandleJump();
        HandlePlatform();
    }

    void HandleMovement()
    {
        // Read WASD
        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            input.y += 1;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1;

        // Get camera directions
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Ignore camera's vertical angle
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Camera-relative movement
        Vector3 moveDirection =
            forward * input.y +
            right * input.x;

        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        // Movement
        Vector3 velocity = moveDirection * speed;

        // Add vertical movement
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

        // Face camera direction
        if (forward != Vector3.zero)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(forward);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void HandleJump()
    {
        if (controller.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            doubleJumpAvailable = hasDoubleJump;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Normal jump / coyote time
            if (coyoteTimeCounter > 0f)
            {
                verticalVelocity = Mathf.Sqrt(
                    jumpHeight * -2f * gravity
                );

                coyoteTimeCounter = 0f;
            }
            // Double jump
            else if (doubleJumpAvailable)
            {
                verticalVelocity = Mathf.Sqrt(
                    doubleJumpHeight * -2f * gravity
                );

                doubleJumpAvailable = false;
            }
        }
    }

    void HandleGravity()
    {
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = groundedForce;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    void HandlePlatform()
    {
        // Moving platform
        if (currentPlatform != null)
        {
            if (!controller.isGrounded)
            {
                currentPlatform = null;
            }
            else
            {
                controller.Move(currentPlatform.MovementDelta);
            }
        }

        // Rotating platform
        if (currentRotatingPlatform != null)
        {
            if (!controller.isGrounded)
            {
                currentRotatingPlatform = null;
                return;
            }

            Vector3 offsetFromPivot =
                transform.position - currentRotatingPlatform.transform.position;

            Vector3 rotatedOffset =
                currentRotatingPlatform.RotationDelta * offsetFromPivot;

            Vector3 rotationMovement =
                rotatedOffset - offsetFromPivot;

            controller.Move(rotationMovement);
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y <= 0.5f)
            return;

        MovingPlatform movingPlatform =
            hit.collider.GetComponentInParent<MovingPlatform>();

        if (movingPlatform != null)
        {
            currentPlatform = movingPlatform;
            currentRotatingPlatform = null;
            return;
        }

        RotatingPlatform rotatingPlatform =
            hit.collider.GetComponentInParent<RotatingPlatform>();

        if (rotatingPlatform != null)
        {
            currentRotatingPlatform = rotatingPlatform;
            currentPlatform = null;
        }
    }
}