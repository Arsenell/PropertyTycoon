using UnityEngine;
using System.Collections;

public class GamePlayer : MonoBehaviour
{
    public string TokenName; // The player's token name (e.g., "Car", "Hat")
    public int Money; // The player's current money
    public int playerID;
    public ArrayList OwnedProperties = new ArrayList(); // List of properties owned by the player

    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    // Adjust the player's money by a specified amount
    public void AdjustMoney(int amount)
    {
        Money += amount;
        Debug.Log($"{TokenName}'s money is now {Money}.");
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        Debug.Log($"Player {playerID} received ${amount}. Total money: ${Money}.");
    }

    public bool DeductMoney(int amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            Debug.Log($"Player {playerID} paid ${amount}. Remaining money: ${Money}.");
            return true;
        }
        else
        {
            Debug.Log($"Player {playerID} doesn't have enough money to pay ${amount}.");
            return false;
        }
    }

    // Add a property to the player's owned properties
    public void AddProperty(Housing property)
    {
        if (property != null)
        {
            OwnedProperties.Add(property);
            Debug.Log($"{TokenName} now owns {property.Name}.");
        }
    }

    // Pay rent to another player
    public void payRent(int amount, GamePlayer owner)
    {
        if (Money >= amount)
        {
            AdjustMoney(-amount);
            owner.AdjustMoney(amount);
            Debug.Log($"{TokenName} paid {amount} in rent to {owner.TokenName}.");
        }
        else
        {
            Debug.Log($"{TokenName} does not have enough money to pay {amount} in rent to {owner.TokenName}.");
        }
    }

    // Display all properties owned by the player
    public void DisplayOwnedProperties()
    {
        Debug.Log($"{TokenName} owns the following properties:");
        foreach (Housing property in OwnedProperties)
        {
            Debug.Log($"- {property.Name}");
        }
    }

    // Buy a property
    public bool buyProperty(Housing property)
    {
        if (property == null)
        {
            Debug.LogError("Property is null. Cannot buy property.");
            return false;
        }

        if (!property.CanBeBought)
        {
            Debug.Log($"Property {property.Name} cannot be bought.");
            return false;
        }

        if (Money < property.Price)
        {
            Debug.Log($"{TokenName} does not have enough money to buy {property.Name}.");
            return false;
        }

        // Deduct money and assign ownership
        AdjustMoney(-property.Price);
        property.Owner = this; // Assign ownership to the current player
        AddProperty(property);

        Debug.Log($"{TokenName} bought {property.Name} for {property.Price}.");
        return true;
    }

    public void SetSprite(Sprite sprite)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer component is missing on the player GameObject.");
                return;
            }
        }

        spriteRenderer.sprite = sprite;
        Debug.Log($"{TokenName} has been assigned a new sprite.");
    }
}