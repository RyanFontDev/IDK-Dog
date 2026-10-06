using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    public Transform[] points;
    public float speed = 2f;
    public float waitTime = 0.5f;

    private int currentPoint = 0;
    private float waitTimer = 0f;
    private bool waiting = false;

    private Vector3[] pointPositions;
    private Vector3 previousPosition;

    public Vector3 MovementDelta { get; private set; }

    void Start()
    {
        if (points.Length < 2)
            return;

        pointPositions = new Vector3[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            pointPositions[i] = points[i].position;
        }

        transform.position = pointPositions[0];

        currentPoint = 1;

        previousPosition = transform.position;
    }

    void Update()
    {
        if (points.Length < 2)
            return;

        if (waiting)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                waiting = false;
            }
        }
        else
        {
            Vector3 targetPosition = pointPositions[currentPoint];

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;

                currentPoint++;

                if (currentPoint >= pointPositions.Length)
                {
                    currentPoint = 0;
                }

                if (waitTime > 0f)
                {
                    waiting = true;
                    waitTimer = waitTime;
                }
            }
        }

        // Calculate how far the platform moved this frame
        MovementDelta = transform.position - previousPosition;

        previousPosition = transform.position;
    }
}