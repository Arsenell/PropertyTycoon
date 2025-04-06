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
        GamePlayer player = other.GetComponent<GamePlayer>();
        if (player == null || property == null)
        {
            return;
        }

        Debug.Log($"Player {player.playerID} landed on {property.Name}.");

        if (property.IsSpecialProperty())
        {
            Debug.Log($"Special property {property.Name} triggered. Handle special logic here.");
            // Add logic for special properties (e.g., pay tax, draw card, etc.)
        }
        else if (property.Owner == null && property.CanBeBought)
        {
            Debug.Log($"Property {property.Name} is available for purchase.");
            // Handle purchase logic
        }
        else if (property.Owner != null && property.Owner != player)
        {
            Debug.Log($"Property {property.Name} is owned by {property.Owner.TokenName}. Player {player.playerID} must pay rent.");
            // Handle rent payment
        }
        else if (property.Owner == player)
        {
            Debug.Log($"Player {player.playerID} landed on their own property: {property.Name}.");
        }
    }
}