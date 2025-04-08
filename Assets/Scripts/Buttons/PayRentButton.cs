using UnityEngine;

public class PayRentButton : MonoBehaviour
{
    private Housing propertyToPayRentFor; // The property where rent is due
    private GamePlayer currentPlayer; // The player who needs to pay rent
    private GameControl gameControl; // Reference to the GameControl instance

    void Start()
    {
        // Find the GameControl instance in the scene
        gameControl = GameControl.Instance;

        if (gameControl == null)
        {
            Debug.LogError("PayRentButton: GameControl instance not found in the scene!");
        }
    }

    // Method to set the property and player for rent payment
    public void SetRentDetails(Housing property, GamePlayer player)
    {
        propertyToPayRentFor = property;
        currentPlayer = player;
    }

    // Called when the button is clicked
    private void OnMouseDown()
    {
        if (propertyToPayRentFor != null && currentPlayer != null && propertyToPayRentFor.Owner != null)
        {
            int rentAmount = CalculateRent(propertyToPayRentFor);
            Debug.Log($"{currentPlayer.TokenName} is paying ${rentAmount} in rent to {propertyToPayRentFor.Owner.TokenName}.");

            // Deduct rent from the current player
            if (currentPlayer.DeductMoney(rentAmount))
            {
                // Add rent to the property owner's balance
                propertyToPayRentFor.Owner.AddMoney(rentAmount);
                Debug.Log($"{currentPlayer.TokenName} successfully paid ${rentAmount} in rent to {propertyToPayRentFor.Owner.TokenName}.");
            }
            else
            {
                Debug.LogError($"{currentPlayer.TokenName} does not have enough money to pay the rent!");
                // Handle bankruptcy logic here if needed
            }
        }
        else
        {
            Debug.LogWarning("PayRentButton: No rent payment is required at the moment.");
        }
    }

    // Helper method to calculate the rent for the property
    private int CalculateRent(Housing property)
    {
        // Assuming rent is based on the first value in the Rent array
        return property.Rent.Length > 0 ? property.Rent[0] : 0;
    }
}
