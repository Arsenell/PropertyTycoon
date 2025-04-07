using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuctionManager : MonoBehaviour
{
    public GameObject auctionPanel;
    public TMP_Text propertyName;
    public TMP_Text highestBidText;
    public TMP_InputField bidInputField;
    public TMP_Text timerText;

    private int highestBid = 0;
    private string highestBidder = "";
    private GamePlayer highestBidderPlayer = null; // Reference to the highest bidder
    private Housing auctionedProperty = null; // The property being auctioned
    private float timer = 30f;
    private bool isAuctionRunning = false;
    // Singleton instance
    public static AuctionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of AuctionManager detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    public void StartAuction(Housing property)
    {
        auctionedProperty = property;
        propertyName.text = property.Name;
        highestBid = 0;
        highestBidder = "";
        highestBidderPlayer = null;
        highestBidText.text = "Highest Bid: £0";
        timer = 30f;
        isAuctionRunning = true;
        Debug.Log($"Auction started ");
        auctionPanel.SetActive(true);

        Debug.Log($"Auction started for {property.Name}.");
    }

    public void SubmitBid(GamePlayer player)
    {
        if (int.TryParse(bidInputField.text, out int bidAmount))
        {
            if (bidAmount > highestBid && player.Money >= bidAmount)
            {
                highestBid = bidAmount;
                highestBidder = player.TokenName;
                highestBidderPlayer = player;
                highestBidText.text = $"Highest Bid: £{highestBid} by {highestBidder}";
                Debug.Log($"{player.TokenName} placed a bid of £{bidAmount}.");
            }
            else if (player.Money < bidAmount)
            {
                Debug.LogWarning($"{player.TokenName} tried to bid £{bidAmount}, but doesn't have enough money.");
            }
        }
        bidInputField.text = "";
    }

    void Update()
    {
        if (!isAuctionRunning) return;

        timer -= Time.deltaTime;
        timerText.text = $"Time Left: {Mathf.CeilToInt(timer)}s";

        if (timer <= 0)
        {
            EndAuction();
        }
    }

    void EndAuction()
    {
        isAuctionRunning = false;
        auctionPanel.SetActive(false);

        if (highestBidderPlayer != null)
        {
            // Deduct the bid amount from the highest bidder
            highestBidderPlayer.AdjustMoney(-highestBid);

            // Transfer ownership of the property
            auctionedProperty.Owner = highestBidderPlayer;
            highestBidderPlayer.AddProperty(auctionedProperty);

            Debug.Log($"{highestBidder} won the auction for {auctionedProperty.Name} with a bid of £{highestBid}.");
        }
        else
        {
            Debug.Log("Auction ended with no bids.");
        }

        // Notify GameControl or other systems if needed
        NotifyAuctionEnd();
    }

    void NotifyAuctionEnd()
    {
        Debug.Log("Auction has ended. Notify other systems if necessary.");
        // Add any additional logic here if GameControl or other systems need to be updated
    }
}
