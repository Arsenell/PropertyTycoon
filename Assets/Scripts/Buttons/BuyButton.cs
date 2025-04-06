using UnityEngine;

public class BuyButton : MonoBehaviour
{
    private Housing propertyToBuy; // Reference to the property being bought
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

    // Method to set the property to buy
    public void SetProperty(Housing property)
    {
        propertyToBuy = property;
    }

    // Called when the button is clicked
    private void OnMouseDown()
    {
        if (propertyToBuy != null)
        {
            Debug.Log($"Buy button clicked for property: {propertyToBuy.Name}");
            bool success = housingManager.BuyProperty(propertyToBuy, GameControl.Instance.currentPlayer);

            if (success)
            {
                Debug.Log($"{GameControl.Instance.currentPlayer.TokenName} successfully bought {propertyToBuy.Name} for {propertyToBuy.Price}.");
            }
            else
            {
                Debug.Log($"{GameControl.Instance.currentPlayer.TokenName} could not afford {propertyToBuy.Name}.");
            }
        }
        else
        {
            Debug.LogError("BuyButton: No property set for purchase.");
        }
    }
}
