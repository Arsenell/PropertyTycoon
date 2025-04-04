using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuctionManager : MonoBehaviour
{
    public GameObject auctionPanel;
    public TMP_Text propertyName;
   // public Image propertyImage;
    public TMP_Text highestBidText;
    public TMP_InputField bidInputField;
    public TMP_Text timerText;

    private int highestBid = 0;
    private string highestBidder = "";
    private float timer = 30f;
    private bool isAuctionRunning = false;

    public void StartAuction(string name)
    {
        propertyName.text = name;
       // propertyImage.sprite = image;
        highestBid = 0;
        highestBidText.text = "Highest Bid: £0";
        timer = 30f;
        isAuctionRunning = true;
        auctionPanel.SetActive(true);
    }

    public void SubmitBid(string playerName)
    {
        if (int.TryParse(bidInputField.text, out int bidAmount))
        {
            if (bidAmount > highestBid)
            {
                highestBid = bidAmount;
                highestBidder = playerName;
                highestBidText.text = $"Highest Bid: £{highestBid} by {playerName}";
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
        timerText.text = $"Auction Over! {highestBidder} wins for £{highestBid}";
        // TODO: Transfer property logic
    }
}
