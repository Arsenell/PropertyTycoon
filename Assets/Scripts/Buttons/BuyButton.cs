using UnityEngine;

public class BuyButton : MonoBehaviour
{
    private Housing propertyToBuy; // Reference to the property being bought
    private GamePlayer currentPlayer; // Reference to the current player
    private HousingManager housingManager; // Reference to the HousingManager

    void Start()
    {
        // Assign the HousingManager instance
        housingManager = GameObject.FindObjectOfType<HousingManager>();

        if (housingManager == null)
        {
            Debug.LogError("BuyButton: HousingManager instance is not found!");
        }
    }

    // Method to set the property and the current player
    public void SetProperty(Housing property, GamePlayer player)
    {
        propertyToBuy = property;
        currentPlayer = player;
    }

    // Called when the button is clicked
    private void OnMouseDown()
    {
        if (propertyToBuy != null && currentPlayer != null)
        {
            Debug.Log($"Buy button clicked for property: {propertyToBuy.Name}");
            bool success = housingManager.BuyProperty(propertyToBuy, currentPlayer);

            if (success)
            {
                Debug.Log($"{currentPlayer.TokenName} successfully bought {propertyToBuy.Name} for {propertyToBuy.Price}.");
            }
            else
            {
                Debug.Log($"{currentPlayer.TokenName} could not afford {propertyToBuy.Name}.");
            }
        }
        else
        {
            Debug.LogError("BuyButton: No property or player set for purchase.");
        }
    }
}
