using System.Collections;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    public Transform[] Waypoints; // Array of waypoints
    public int currentWaypointIndex = 0; // Current waypoint index

    public void MovePlayer(int diceSideThrown, System.Action onMovementComplete)
    {
        StartCoroutine(MovePlayerCoroutine(diceSideThrown, onMovementComplete));
    }

    private IEnumerator MovePlayerCoroutine(int diceSideThrown, System.Action onMovementComplete)
    {
        // Add an offset of 1 to the movement
        currentWaypointIndex = (currentWaypointIndex + 1) % Waypoints.Length;

        for (int i = 0; i < diceSideThrown; i++)
        {
            // Move to the next waypoint
            transform.position = Waypoints[currentWaypointIndex].position;
            currentWaypointIndex = (currentWaypointIndex + 1) % Waypoints.Length;

            Debug.Log($"Player moved to waypoint index: {currentWaypointIndex}, position: {Waypoints[currentWaypointIndex].position}");

            // Wait for a short delay to simulate movement
            yield return new WaitForSeconds(0.5f);
        }

        // Ensure the player's position matches the final waypoint
        transform.position = Waypoints[currentWaypointIndex].position;
        Debug.Log($"Player final position: {transform.position}");

        // Trigger the callback after movement is complete
        onMovementComplete?.Invoke();
    }

    public int getCurrentWaypointIndex()
    {
        return currentWaypointIndex;
    }
}