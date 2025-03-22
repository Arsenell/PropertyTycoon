using UnityEngine;

public class GameControl : MonoBehaviour
{
    private static GameObject player1, player2;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    void Start() {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        if (player1 == null) {
            Debug.LogError("Player1 not found!");
        }
        if (player2 == null) {
            Debug.LogError("Player2 not found!");
        }
    }

    void Update() {
        if (player1 != null && player1.GetComponent<WaypointMover>().waypointIndex > 0) {
            player1StartWaypoint = player1.GetComponent<PlayerMovement>().waypointIndex;
        }
        if (player2 != null && player2.GetComponent<PlayerMovement>().waypointIndex > 0) {
            player2StartWaypoint = player2.GetComponent<PlayerMovement>().waypointIndex;
        }
    }

    public static void MovePlayer(int playerToMove) {
        if (playerToMove == 1 && player1 != null) {
            player1.GetComponent<PlayerMovement>().MovePlayer(diceSideThrown);
        } else if (playerToMove == 2 && player2 != null) {
            player2.GetComponent<PlayerMovement>().MovePlayer(diceSideThrown);
        }
    }
    public static void gameOver(string message) {
    Debug.Log(message); // Display the game over message in the console
    Time.timeScale = 0; // Pause the game by setting the time scale to 0
}
}
