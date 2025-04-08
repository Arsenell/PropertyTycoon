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
    private int freeParkingMoney = 0; // Accumulated money for Free Parking
    private const int GO_REWARD = 200; // Reward for passing GO
    private const int INCOME_TAX_AMOUNT = 100; // Fixed income tax amount
    private bool isTurnActive = true; // Tracks if the current player's turn is active

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
                return; // Do not switch turns here; the player is in jail
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
            HandleSpecialSpaces(playerToMove, player.GetComponent<WaypointMover>().getCurrentWaypointIndex());
            HandleRentPayment(playerToMove); // Check if rent needs to be paid
            HandlePropertyPurchase(playerToMove); // Handle property purchase after moving
            if (!isDouble) SwitchTurn(); // Switch to the next player's turn if no double
        });
    }

    public void SendPlayerToJail(int playerToMove)
    {
        GamePlayer player = players[playerToMove - 1]?.GetComponent<GamePlayer>();
        if (player != null)
        {
            Debug.Log($"Player {playerToMove} is sent to jail.");

            // Assuming the jail waypoint index is predefined (e.g., 10)
            int jailWaypointIndex = 10; // Replace with the actual index of the jail waypoint
            player.GetComponent<WaypointMover>().MoveToJail(jailWaypointIndex);
        }
    }

    private void SwitchTurn()
    {
        do
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length; // Wrap around to Player 1 after the last player
        } while (players[currentPlayerIndex] == null); // Skip bankrupt players

        isTurnActive = true; // Reset the turn state for the next player
        Debug.Log($"Now it's Player {currentPlayerIndex + 1}'s turn!");
    }

    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    public void HandlePropertyPurchase(int playerID)
    {
        if (!isTurnActive)
        {
            Debug.LogWarning("Property purchase attempted, but the turn is not active.");
            return;
        }

        GamePlayer player = players[playerID - 1]?.GetComponent<GamePlayer>();
        if (player == null)
        {
            Debug.LogError($"Player {playerID} is null or does not exist.");
            return;
        }

        Housing landedProperty = GetLandedProperty(player); // Get the property the player landed on
        if (landedProperty != null && !landedProperty.IsOwned)
        {
            Debug.Log($"Player {playerID} landed on {landedProperty.Name}. Setting up BuyButton.");
            
            // Find the BuyButton instance and set the property and player
            BuyButton buyButton = FindObjectOfType<BuyButton>();
            if (buyButton != null)
            {
                buyButton.SetProperty(landedProperty, player);
            }
            else
            {
                Debug.LogError("BuyButton instance not found in the scene!");
            }
        }
        else
        {
            Debug.Log($"Player {playerID} landed on a property that is either owned or not purchasable.");
        }
    }

    public void HandleRentPayment(int playerID)
    {
        GamePlayer player = players[playerID - 1]?.GetComponent<GamePlayer>();
        if (player == null)
        {
            Debug.LogError($"Player {playerID} is null or does not exist.");
            return;
        }

        Housing landedProperty = GetLandedProperty(player); // Get the property the player landed on
        if (landedProperty != null && landedProperty.Owner != null && landedProperty.Owner != player)
        {
            Debug.Log($"Player {playerID} landed on {landedProperty.Name}, owned by {landedProperty.Owner.TokenName}. Setting up PayRentButton.");

            // Find the PayRentButton instance and set the rent details
            PayRentButton payRentButton = FindObjectOfType<PayRentButton>();
            if (payRentButton != null)
            {
                payRentButton.SetRentDetails(landedProperty, player);
            }
            else
            {
                Debug.LogError("PayRentButton instance not found in the scene!");
            }
        }
        else
        {
            Debug.Log($"Player {playerID} landed on a property that is not owned or does not require rent payment.");
        }
    }

    // Helper method to get the property the player landed on
    private Housing GetLandedProperty(GamePlayer player)
    {
        // Assuming the player's current waypoint corresponds to a property
        int currentWaypointIndex = player.GetComponent<WaypointMover>().getCurrentWaypointIndex();
        return housingManager.GetPropertyAtWaypoint(currentWaypointIndex); // Replace with your logic to fetch the property
    }

    public void HandleSpecialSpaces(int playerToMove, int currentWaypointIndex)
    {
        GamePlayer player = players[playerToMove - 1]?.GetComponent<GamePlayer>();
        if (player == null)
        {
            Debug.LogError($"Player {playerToMove} is null or does not exist.");
            return;
        }

        Card potLuckCard = null; // Declare potLuckCard outside the switch
        Card opportunityKnocksCard = null; // Declare opportunityKnocksCard outside the switch

        switch (currentWaypointIndex)
        {
            case 0: // GO
                Debug.Log($"Player {playerToMove} passed GO! Collecting ${GO_REWARD}.");
                player.AddMoney(GO_REWARD);
                break;

            case 4: // Income Tax
                Debug.Log($"Player {playerToMove} landed on Income Tax. Paying ${INCOME_TAX_AMOUNT}.");
                if (player.DeductMoney(INCOME_TAX_AMOUNT))
                {
                    freeParkingMoney += INCOME_TAX_AMOUNT; // Add to Free Parking pool
                    Debug.Log($"${INCOME_TAX_AMOUNT} added to Free Parking pool. Total: ${freeParkingMoney}.");
                }
                else
                {
                    Debug.Log($"Player {playerToMove} doesn't have enough money to pay Income Tax.");
                    HandleBankruptcy(playerToMove - 1);
                }
                break;

            case 2: // Pot Luck
            case 17: // Pot Luck
            case 36: // Pot Luck
                Debug.Log($"Player {playerToMove} landed on Pot Luck.");
                potLuckCard = FindObjectOfType<CardManager>().DrawPotLuckCard();
                FindObjectOfType<CardManager>().ExecuteCardAction(potLuckCard, player);
                break;

            case 10: // Go To Jail
                Debug.Log($"Player {playerToMove} landed on Go To Jail. Sending to jail.");
                SendPlayerToJail(playerToMove);
                break;

            case 20: // Free Parking
                Debug.Log($"Player {playerToMove} landed on Free Parking. Collecting ${freeParkingMoney}.");
                player.AddMoney(freeParkingMoney);
                freeParkingMoney = 0; // Reset Free Parking pool
                break;

            case 22: // Opportunity Knocks
            case 33: // Opportunity Knocks
                Debug.Log($"Player {playerToMove} landed on Opportunity Knocks.");
                opportunityKnocksCard = FindObjectOfType<CardManager>().DrawOpportunityKnocksCard();
                FindObjectOfType<CardManager>().ExecuteCardAction(opportunityKnocksCard, player);
                break;

            default:
                Debug.Log($"Player {playerToMove} landed on a regular space.");
                break;
        }
    }

    public void EndTurn()
    {
        if (!isTurnActive)
        {
            Debug.LogWarning("EndTurn called, but the turn is already inactive.");
            return;
        }

        isTurnActive = false; // Mark the turn as inactive
        SwitchTurn(); // Move to the next player's turn
    }

    public bool IsTurnActive()
    {
        return isTurnActive;
    }

    public void AddToFreeParking(int amount)
    {
        freeParkingMoney += amount;
        Debug.Log($"Added ${amount} to Free Parking. Total Free Parking money: ${freeParkingMoney}.");
    }

    public void HandleBankruptcy(int playerIndex)
    {
        GamePlayer player = players[playerIndex]?.GetComponent<GamePlayer>();
        if (player == null)
        {
            Debug.LogError($"Player {playerIndex + 1} is null or does not exist.");
            return;
        }

        Debug.Log($"Player {playerIndex + 1} ({player.TokenName}) has declared bankruptcy and is removed from the game.");

        // Remove the player from the game
        players[playerIndex] = null;

        // Check if only one player remains
        int remainingPlayers = 0;
        int lastPlayerIndex = -1;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                remainingPlayers++;
                lastPlayerIndex = i;
            }
        }

        if (remainingPlayers == 1)
        {
            DeclareWinner(lastPlayerIndex);
        }
        else
        {
            // Move to the next player's turn
            SwitchTurn();
        }
    }

    private void DeclareWinner(int playerIndex)
    {
        GamePlayer winner = players[playerIndex]?.GetComponent<GamePlayer>();
        if (winner != null)
        {
            Debug.Log($"Player {playerIndex + 1} ({winner.TokenName}) is the winner!");
            isGameOver = true;

            // Display a message or trigger an end-game screen
            // Example: Show a UI panel with the winner's name
            Debug.Log("Game Over! Congratulations to the winner!");
        }
    }
}