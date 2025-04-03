using UnityEngine;

public class GameControl : MonoBehaviour
{
    private static GameObject player1, player2;
    public static bool isGameOver = false;
    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    private HousingManager housingManager;

    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        housingManager = GameObject.Find("HousingManager").GetComponent<HousingManager>();

        if (player1 == null || player2 == null || housingManager == null)
        {
            Debug.LogError("GameControl: Missing references to players or HousingManager!");
        }
    }

    void Update()
    {
        if (player1 != null && player1.GetComponent<WaypointMover>().getCurrnetWaypointIndex() > 0)
        {
            player1StartWaypoint = player1.GetComponent<WaypointMover>().getCurrnetWaypointIndex();
        }
        if (player2 != null && player2.GetComponent<WaypointMover>().getCurrnetWaypointIndex() > 0)
        {
            player2StartWaypoint = player2.GetComponent<WaypointMover>().getCurrnetWaypointIndex();
        }
    }

    public void processProperty(int playerID, int propertyID)
    {
        Housing property = housingManager.Properties[propertyID];
        Player player = (playerID == 1) ? player1.GetComponent<Player>() : player2.GetComponent<Player>();

        if (property.CanBeBought)
        {
            if (player != null)
            {
                bool success = housingManager.BuyProperty(property, player);
                if (success)
                {
                    Debug.Log($"Player {playerID} successfully bought {property.Name}.");
                }
            }
        }
        else if (property.Owner != null && property.Owner != player)
        {
            int rent = property.GetRent(0); // Assuming 0 houses for simplicity
            player.payRent(rent, property.Owner);
        }
    }

    public static void MovePlayer(int playerToMove, int diceSideThrown)
    {
        if (playerToMove == 1 && player1 != null)
        {
            player1.GetComponent<WaypointMover>().MovePlayer(diceSideThrown);
        }
        else if (playerToMove == 2 && player2 != null)
        {
            player2.GetComponent<WaypointMover>().MovePlayer(diceSideThrown);
        }
    }

    public static bool GameOver(string message)
    {
        Debug.Log(message);
        Time.timeScale = 0;
        isGameOver = true;
        return true;
    }
}
