using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Animation")]
    public float rotationSpeed = 90f;
    public float bobHeight = 0.25f;
    public float bobSpeed = 2f;

    private bool collected = false;

    private Vector3 startPosition;
    private float bobOffset;

    void Start()
    {
        startPosition = transform.position;

        // Randomize the starting point of the bobbing
        bobOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        // Spin around the Y axis
        transform.Rotate(
            0f,
            rotationSpeed * Time.deltaTime,
            0f
        );

        // Bob up and down
        float newY = startPosition.y +
                     Mathf.Sin(Time.time * bobSpeed + bobOffset) * bobHeight;

        transform.position = new Vector3(
            startPosition.x,
            newY,
            startPosition.z
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            collected = true;

            CollectibleManager.Instance.Collect();

            Destroy(gameObject);
        }
    }
}