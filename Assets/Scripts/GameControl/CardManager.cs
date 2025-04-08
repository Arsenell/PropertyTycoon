using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public TextAsset potLuckCSV; // CSV file for Pot Luck cards
    public TextAsset opportunityKnocksCSV; // CSV file for Opportunity Knocks cards

    private Queue<Card> potLuckCards = new Queue<Card>();
    private Queue<Card> opportunityKnocksCards = new Queue<Card>();

    void Start()
    {
        LoadCards(potLuckCSV, potLuckCards);
        LoadCards(opportunityKnocksCSV, opportunityKnocksCards);
    }

    private void LoadCards(TextAsset csvFile, Queue<Card> cardQueue)
    {
        string[] lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; i++) // Skip the header row
        {
            string[] columns = lines[i].Split(',');
            if (columns.Length >= 2)
            {
                string description = columns[0].Trim('"');
                string action = columns[3].Trim();
                cardQueue.Enqueue(new Card(description, action));
            }
        }
    }

    public Card DrawPotLuckCard()
    {
        if (potLuckCards.Count == 0) ReloadDeck(potLuckCards, potLuckCSV);
        return potLuckCards.Dequeue();
    }

    public Card DrawOpportunityKnocksCard()
    {
        if (opportunityKnocksCards.Count == 0) ReloadDeck(opportunityKnocksCards, opportunityKnocksCSV);
        return opportunityKnocksCards.Dequeue();
    }

    private void ReloadDeck(Queue<Card> cardQueue, TextAsset csvFile)
    {
        Debug.Log("Reloading deck...");
        LoadCards(csvFile, cardQueue);
    }

    public void ExecuteCardAction(Card card, GamePlayer player)
    {
        Debug.Log($"Executing card: {card.Description}");

        string action = card.Action.ToLower();
        if (action.Contains("bank pays player"))
        {
            int amount = ExtractAmount(action);
            player.AddMoney(amount);
            Debug.Log($"Bank paid {player.TokenName} ${amount}.");
        }
        else if (action.Contains("player pays"))
        {
            int amount = ExtractAmount(action);
            if (player.DeductMoney(amount))
            {
                Debug.Log($"{player.TokenName} paid ${amount} to the bank.");
            }
            else
            {
                Debug.LogError($"{player.TokenName} does not have enough money to pay ${amount}!");
                // Handle bankruptcy logic here
            }
        }
        else if (action.Contains("player moves forwards to"))
        {
            string destination = ExtractDestination(action);
            MovePlayerToDestination(player, destination);
        }
        else if (action.Contains("player moves backwards"))
        {
            int spaces = ExtractSpaces(action);
            MovePlayerBackwards(player, spaces);
        }
        else if (action.Contains("go to jail"))
        {
            GameControl.Instance.SendPlayerToJail(player.playerID); // Updated to use playerID
        }
        else if (action.Contains("free parking"))
        {
            int amount = ExtractAmount(action);
            GameControl.Instance.AddToFreeParking(amount);
        }
        else
        {
            Debug.LogWarning($"Unknown action: {card.Action}");
        }
    }

    private int ExtractAmount(string action)
    {
        string[] words = action.Split(' ');
        foreach (string word in words)
        {
            if (int.TryParse(word.Trim('£', '$'), out int amount))
            {
                return amount;
            }
        }
        return 0;
    }

    private string ExtractDestination(string action)
    {
        int index = action.IndexOf("to");
        return index >= 0 ? action.Substring(index + 3).Trim() : "";
    }

    private int ExtractSpaces(string action)
    {
        string[] words = action.Split(' ');
        foreach (string word in words)
        {
            if (int.TryParse(word, out int spaces))
            {
                return spaces;
            }
        }
        return 0;
    }

    private void MovePlayerToDestination(GamePlayer player, string destination)
    {
        Debug.Log($"Moving {player.TokenName} to {destination}.");
        // Implement logic to move the player to the specified destination
    }

    private void MovePlayerBackwards(GamePlayer player, int spaces)
    {
        Debug.Log($"Moving {player.TokenName} backwards by {spaces} spaces.");
        // Implement logic to move the player backwards
    }
}

public class Card
{
    public string Description { get; private set; }
    public string Action { get; private set; }

    public Card(string description, string action)
    {
        Description = description;
        Action = action;
    }
}
