using UnityEngine;
using UnityEngine.InputSystem;

public class CamMovement : MonoBehaviour
{
    public Transform target;
    public float lerpFactor = .2f;

    public Vector2 mouseInput;
    public Vector2 camRot;

    InputAction lookAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        mouseInput = lookAction.ReadValue<Vector2>();
        camRot.x += mouseInput.x / 10;
        camRot.y -= mouseInput.y / 10;
        camRot.y = Mathf.Clamp(camRot.y, -20, 70);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(camRot.y, camRot.x, 0);
        transform.position = Vector3.Lerp(transform.position, target.position, lerpFactor);
    }
}