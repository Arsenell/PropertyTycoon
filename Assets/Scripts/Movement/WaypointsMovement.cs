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

    void Update()
    {
        // Move towards the current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // Check if the object has reached the waypoint
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // Move to the next waypoint (loop back to the first if at the end)
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}