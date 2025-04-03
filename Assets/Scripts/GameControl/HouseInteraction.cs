using UnityEngine;

public class HouseInteraction : MonoBehaviour
{
    private Housing property; // The property associated with this waypoint

    /// <summary>
    /// Initializes the interaction script with the associated property.
    /// </summary>
    /// <param name="property">The Housing object associated with this waypoint.</param>
    public void Initialize(Housing property)
    {
        this.property = property;
    }

    /// <summary>
    /// Triggered when a player lands on this waypoint.
    /// </summary>
    /// <param name="other">The collider of the player GameObject.</param>
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player == null || property == null)
        {
            return; // Exit if no player or property is associated
        }

        Debug.Log($"Player {player.playerID} landed on {property.Name}.");

        // Handle property interaction
        if (property.Owner == null && property.CanBeBought)
        {
            // Property is unowned and can be bought
            Debug.Log($"Property {property.Name} is available for purchase.");
            player.buyProperty(property);
        }
        else if (property.Owner != null && property.Owner != player)
        {
            // Property is owned by another player, pay rent
            int rent = property.GetRent(0); // Assuming 0 houses for simplicity
            Debug.Log($"Property {property.Name} is owned by Player {property.Owner.playerID}. Rent is {rent}.");
            player.payRent(rent, property.Owner);
        }
        else if (property.Owner == player)
        {
            // Player owns this property
            Debug.Log($"Player {player.playerID} landed on their own property: {property.Name}.");
        }
    }
}