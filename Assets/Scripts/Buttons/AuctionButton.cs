using UnityEngine;

public class AuctionButton : MonoBehaviour
{
    // Reference to the property being auctioned
    private Housing propertyToAuction;
    private AuctionManager auctionManager;

    void Start()
    {
        // Assign the AuctionManager instance
        auctionManager = AuctionManager.Instance;

        if (auctionManager == null)
        {
            Debug.LogError("AuctionButton: AuctionManager instance is not found!");
        }
    }

    // Method to set the property to auction
    public void SetProperty(Housing property)
    {
        propertyToAuction = property;
    }

    // Called when the button is clicked
    private void OnMouseDown()
    {
        if (propertyToAuction != null)
        {
            if (auctionManager != null)
            {
                Debug.Log($"Auction button clicked for property: {propertyToAuction.Name}");
                auctionManager.StartAuction(propertyToAuction);
            }
            else
            {
                Debug.LogError("AuctionButton: AuctionManager is not assigned.");
            }
        }
        else
        {
            Debug.LogError("AuctionButton: No property set for auction.");
        }
    }
}
