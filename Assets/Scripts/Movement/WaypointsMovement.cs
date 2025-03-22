using System.Collections;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints
    public float speed = 2f; // Movement speed
    private int currentWaypointIndex = 0;

    void Start()
    {
        // Ensure waypoints array is not null or empty
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("Waypoints array is empty or not assigned.");
            enabled = false; // Disable the script to avoid unnecessary updates
        }
    }

    public int getCurrnetWaypointIndex()
    {
        return currentWaypointIndex;
    }

    public void MovePlayer(int steps)
    {
        StartCoroutine(MovePlayerCoroutine(steps));
    }

    private IEnumerator MovePlayerCoroutine(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];

            // Move towards the target waypoint
            while (Vector2.Distance(transform.position, targetWaypoint.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
                yield return null; // Wait for the next frame
            }

            // Update to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}