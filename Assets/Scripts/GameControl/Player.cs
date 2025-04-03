using UnityEngine;
using System.Collections.Generic;

/*
The player will need to be able to buy properties, houses and hotels. done
The player will also need to be able to mortgage properties and sell houses and hotels.
The player will also need to be able to trade properties with other players.
The player will also need to be able to pay rent to other players.
The player will also need to be able to pay taxes.

These are the functionalities that the player will need to do
*/
public class Player : MonoBehaviour
{
    public int playerID;
    public string TokenName;
    public int Money = 1500;
    private List<Housing> propertiesOwned = new List<Housing>();

    void Start()
    {
        Debug.Log($"Player {playerID} initialized with {Money} money.");
    }

    public void buyProperty(Housing property)
    {
        if (property.CanBeBought && Money >= property.Price)
        {
            propertiesOwned.Add(property);
            AdjustMoney(-property.Price); // Use the new method to deduct money
            property.Owner = this;
            Debug.Log($"Player {playerID} bought property: {property.Name}");
        }
        else
        {
            Debug.Log("Not enough money or property cannot be bought.");
        }
    }

    public void payRent(int rentAmount, Player owner)
    {
        if (Money >= rentAmount)
        {
            AdjustMoney(-rentAmount); // Deduct rent from this player
            owner.AdjustMoney(rentAmount); // Add rent to the owner
            Debug.Log($"Player {playerID} paid {rentAmount} in rent to Player {owner.playerID}.");
        }
        else
        {
            Debug.Log($"Player {playerID} cannot afford to pay rent of {rentAmount}.");
        }
    }

    public void AdjustMoney(int amount)
    {
        Money += amount;
        Debug.Log($"Player {playerID}'s money updated to {Money}.");
    }
}
