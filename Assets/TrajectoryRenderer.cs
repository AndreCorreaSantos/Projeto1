using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryRenderer : MonoBehaviour
{
    public Rigidbody ballRigidbody; // Assign this in the editor or find it during Start/Awake
    public LineRenderer trajectoryLineRenderer; // Assign this in the editor or find it during Start/Awake
    public int lineSegmentCount = 20; // Number of line segments to calculate - more segments result in a smoother line
    public float launchForce = 1f; // The force that will be applied to the ball

    private void Reset()
    {
        // Attempt to find the LineRenderer component if not already assigned
        trajectoryLineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        if (!trajectoryLineRenderer)
        {
            trajectoryLineRenderer = GetComponent<LineRenderer>();
        }
        if (!ballRigidbody)
        {
            ballRigidbody = GetComponent<Rigidbody>();
        }

        trajectoryLineRenderer.positionCount = lineSegmentCount;
    }

    public void ShowTrajectory()
    {
        Vector3[] points = new Vector3[lineSegmentCount];
        Vector3 startingPosition = ballRigidbody.position;
        Vector3 startingVelocity = ballRigidbody.transform.forward * launchForce / ballRigidbody.mass;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * Time.fixedDeltaTime;
            points[i] = startingPosition + startingVelocity * time + Physics.gravity * time * time / 2f;
            if (Physics.Raycast(points[i] - new Vector3(0, 0.1f, 0), Vector3.down, out RaycastHit hit, 0.2f))
            {
                // If the raycast hits the ground, set the line end point here and break the loop
                trajectoryLineRenderer.positionCount = i + 1;
                break;
            }
        }

        trajectoryLineRenderer.SetPositions(points);
    }

    public void HideTrajectory()
    {
        trajectoryLineRenderer.positionCount = 0;
    }
}
