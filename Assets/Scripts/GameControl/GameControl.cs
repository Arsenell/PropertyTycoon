using UnityEngine;

public class GameControl : MonoBehaviour
{
    private static GameObject[] players; // Array to hold all player GameObjects
    public static bool isGameOver = false;
    public static int diceSideThrown = 0;

    private HousingManager housingManager;
    private AuctionManager auctionManager;

    private AuctionButton auctionButton;
    private static GameControl instance;

    private int currentPlayerIndex = 0; // Tracks the current player's turn
    private int consecutiveDoubles = 0; // Tracks how many doubles the current player has rolled

    public static GameControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameControl>();
            }
            return instance;
        }
    }

    void Start()
    {
        // Initialize references
        housingManager = GameObject.Find("HousingManager")?.GetComponent<HousingManager>();
        auctionManager = GameObject.Find("AuctionManager")?.GetComponent<AuctionManager>();

        if (housingManager == null || auctionManager == null)
        {
            Debug.LogError("GameControl: Missing references to HousingManager or AuctionManager!");
        }

        // Spawn players
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        int playerCount = GameSettings.playerCount;
        players = new GameObject[playerCount];

        PlayerSpawner playerSpawner = FindObjectOfType<PlayerSpawner>();
        if (playerSpawner == null)
        {
            Debug.LogError("PlayerSpawner is not found in the scene!");
            return;
        }

        // Spawn players using the PlayerSpawner
        playerSpawner.SpawnPlayers();

        // Assign players to the players array
        for (int i = 0; i < playerCount; i++)
        {
            players[i] = GameObject.Find($"Player {i + 1}");
            if (players[i] == null)
            {
                Debug.LogError($"Player {i + 1} was not found after spawning!");
            }
        }

        Debug.Log($"Spawned and assigned {playerCount} players.");
    }

    public void MovePlayer(int playerToMove, int diceSideThrown, bool isDouble)
    {
        if (playerToMove < 1 || playerToMove > players.Length)
        {
            Debug.LogError($"Player {playerToMove} is out of bounds!");
            return;
        }

        GamePlayer player = players[playerToMove - 1]?.GetComponent<GamePlayer>();
        if (player == null)
        {
            Debug.LogError($"Player {playerToMove} is null or does not exist.");
            return;
        }

        if (isDouble)
        {
            consecutiveDoubles++;
            Debug.Log($"Player {playerToMove} rolled a double! Consecutive doubles: {consecutiveDoubles}");

            if (consecutiveDoubles >= 2)
            {
                Debug.Log($"Player {playerToMove} rolled doubles twice in a row! Sending to jail.");
                SendPlayerToJail(playerToMove);
                SwitchTurn();
                return;
            }
        }
        else
        {
            consecutiveDoubles = 0; // Reset doubles counter if no double is rolled
        }

        Debug.Log($"Moving Player {playerToMove} by {diceSideThrown} spaces.");

        // Call the player's movement logic
        player.GetComponent<WaypointMover>().MovePlayer(diceSideThrown, () =>
        {
            HandlePropertyPurchase(playerToMove); // Handle property purchase after moving
            if (!isDouble) SwitchTurn(); // Switch to the next player's turn if no double
        });
    }

    private void SendPlayerToJail(int playerToMove)
    {
        GamePlayer player = players[playerToMove - 1]?.GetComponent<GamePlayer>();
        if (player != null)
        {
            Debug.Log($"Player {playerToMove} is sent to jail.");
            player.GetComponent<WaypointMover>().MoveToJail(); // Assuming MoveToJail is implemented in WaypointMover
        }
    }

    private void SwitchTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length; // Wrap around to Player 1 after the last player
        Debug.Log($"Now it's Player {currentPlayerIndex + 1}'s turn!");
    }

    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    public void HandlePropertyPurchase(int playerID)
    {
        // Logic for handling property purchase
        Debug.Log($"Player {playerID} landed on a property. Handle purchase logic here.");
    }
}