using Unity.Properties;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    private static GameObject player1, player2;
    public static bool isGameOver = false;
    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    private HousingManager housingManager;
    private AuctionManager auctionManager;

    private AuctionButton auctionButton;
    private static GameControl instance;

    public GamePlayer currentPlayer; // Add this attribute to track the current player

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

    // Add logic to set the current player during the game
    public void SetCurrentPlayer(GamePlayer player)
    {
        currentPlayer = player;
    }

    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        housingManager = GameObject.Find("HousingManager")?.GetComponent<HousingManager>();
        auctionManager = GameObject.Find("AuctionManager")?.GetComponent<AuctionManager>();
        if (player1 == null || player2 == null || housingManager == null)
        {
            Debug.LogError("GameControl: Missing references to players or HousingManager!");
        }
    }

    void Update()
    {
        if (player1 != null)
        {
            int waypointIndex = player1.GetComponent<WaypointMover>().getCurrentWaypointIndex();
            if (waypointIndex > 0)
            {
                player1StartWaypoint = waypointIndex;
            }
        }

        if (player2 != null)
        {
            int waypointIndex = player2.GetComponent<WaypointMover>().getCurrentWaypointIndex();
            if (waypointIndex > 0)
            {
                player2StartWaypoint = waypointIndex;
            }
        }
    }

    public void ProcessProperty(int playerID, int propertyID)
    {
        if (housingManager == null)
        {
            Debug.LogError("GameControl: HousingManager is not initialized!");
            return;
        }

        Housing property = housingManager.Properties[propertyID];
        GamePlayer player = (playerID == 1) ? player1.GetComponent<GamePlayer>() : player2.GetComponent<GamePlayer>();

        if (property == null)
        {
            Debug.LogError($"No property found at property ID {propertyID}.");
            return;
        }

        if (player == null)
        {
            Debug.LogError($"Player {playerID} is null. Cannot process property.");
            return;
        }

        if (property.CanBeBought && property.Owner == null)
        {
            Debug.Log($"Player {playerID} landed on {property.Name}, which can be bought for {property.Price}.");
            auctionButton = GameObject.Find("AuctionButton")?.GetComponent<AuctionButton>();
            if (auctionButton == null)
            {
                Debug.LogError("AuctionButton is not found or does not have the AuctionButton component.");
                return;
            }
            auctionButton.SetProperty(property);
            HandleBuyPrompt(player, property);
            HandleBuyOrAuction(player, property);
        }
        else if (property.Owner != null && property.Owner != player)
        {
            int rent = property.GetRent(0); // Assuming 0 houses for simplicity
            player.payRent(rent, property.Owner);
            Debug.Log($"Player {playerID} paid {rent} in rent to {property.Owner.TokenName}.");
        }
        else
        {
            Debug.Log($"Player {playerID} landed on {property.Name}, but it cannot be bought.");
        }
    }

    public void HandlePropertyPurchase(int playerID)
    {
        // Determine the player's current waypoint index
        int currentWaypointIndex = (playerID == 1) ? player1.GetComponent<WaypointMover>().getCurrentWaypointIndex()
                                                   : player2.GetComponent<WaypointMover>().getCurrentWaypointIndex();
        if (currentWaypointIndex < 0 || currentWaypointIndex >= housingManager.Properties.Count)
        {
            Debug.LogError($"Invalid waypoint index {currentWaypointIndex}. Cannot process property.");
            return;
        }
        Debug.Log($"Player {playerID} landed on waypoint {currentWaypointIndex}.");
        
        // Use ProcessProperty to handle the property logic
        ProcessProperty(playerID, currentWaypointIndex);
    }

    public static void MovePlayer(int playerToMove, int diceSideThrown)
    {
        if (Instance == null)
        {
            Debug.LogError("GameControl: Instance is not initialized!");
            return;
        }

        if (playerToMove == 1 && player1 != null)
        {
            player1.GetComponent<WaypointMover>().MovePlayer(diceSideThrown, () =>
            {
                Instance.HandlePropertyPurchase(1); // Handle property purchase after moving
            });
        }
        else if (playerToMove == 2 && player2 != null)
        {
            player2.GetComponent<WaypointMover>().MovePlayer(diceSideThrown, () =>
            {
                Instance.HandlePropertyPurchase(2); // Handle property purchase after moving
            });
        }
        else
        {
            Debug.LogError($"GameControl: Invalid player ID {playerToMove} or player object is null.");
        }
    }

    public static bool GameOver(string message)
    {
        Debug.Log(message);
        Time.timeScale = 0;
        isGameOver = true;
        return true;
    }

    private void HandleBuyOrAuction(GamePlayer player, Housing property)
    {
        Debug.Log($"Prompting {player.TokenName} to buy or auction {property.Name}.");

        // Display the buy/auction UI
        GameObject buyAuctionPanel = GameObject.Find("BuyAuctionPanel");
        if (buyAuctionPanel != null)
        {
            buyAuctionPanel.SetActive(true);

            // Set up the Buy button
            BuyButton buyButton = GameObject.Find("BuyButton")?.GetComponent<BuyButton>();
            if (buyButton != null)
            {
                buyButton.SetProperty(property);
            }
            else
            {
                Debug.LogError("BuyButton is not found or does not have the BuyButton component.");
            }

            // Set up the Auction button
            AuctionButton auctionButton = GameObject.Find("AuctionButton")?.GetComponent<AuctionButton>();
            if (auctionButton != null)
            {
                auctionButton.SetProperty(property);
            }
            else
            {
                Debug.LogError("AuctionButton is not found or does not have the AuctionButton component.");
            }
        }
        else
        {
            Debug.LogError("BuyAuctionPanel is not found in the scene.");
        }
    }

    private void HandleBuyPrompt(GamePlayer player, Housing property)
    {
        // Set the property for the buy button
        BuyButton buyButton = GameObject.Find("BuyButton")?.GetComponent<BuyButton>();
        if (buyButton != null)
        {
            buyButton.SetProperty(property);
        }
        else
        {
            Debug.LogError("BuyButton is not found or does not have the BuyButton component.");
        }
    }

    // Simulate player decision (replace this with actual UI logic)
    private bool SimulatePlayerDecision()
    {
        // Simulate a random decision for now (true = buy, false = auction)
        return Random.value > 0.5f;
    }
}