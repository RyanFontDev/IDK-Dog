using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [Header("Rotation")]
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 45f;

    private Quaternion previousRotation;

    public Quaternion RotationDelta { get; private set; }

    void Start()
    {
        previousRotation = transform.rotation;
    }

    void Update()
    {
        transform.Rotate(
            rotationAxis,
            rotationSpeed * Time.deltaTime,
            Space.World
        );

        RotationDelta = transform.rotation * Quaternion.Inverse(previousRotation);

        previousRotation = transform.rotation;
    }
}