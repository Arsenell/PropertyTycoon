using UnityEngine;
using System.Collections.Generic;

public class HousingManager : MonoBehaviour
{
    public List<Housing> Properties = new List<Housing>();
    public List<Transform> Waypoints; // List of waypoints corresponding to houses

    void Start()
    {
        LoadPropertiesFromCSV("DataForPTycoon");
        AssignWaypointsToProperties();
        PrintProperties();
    }

    void LoadPropertiesFromCSV(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            Debug.LogError("CSV file not found!");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; i++) // Skip the header row
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] fields = lines[i].Split(',');
            if (fields.Length < 10) // Ensure there are at least 10 columns
            {
                Debug.LogWarning($"Skipping invalid row: {lines[i]}");
                continue;
            }

            // Parse property data
            string name = string.IsNullOrWhiteSpace(fields[1]) ? "Unnamed Property" : fields[1].Trim();
            string group = string.IsNullOrWhiteSpace(fields[3]) ? "No Group" : fields[3].Trim();
            bool canBeBought = fields[5].Trim().ToLower() == "yes";

            int price = 0;
            if (!int.TryParse(fields[7], out price))
            {
                Debug.LogWarning($"Invalid price for property '{name}', defaulting to 0.");
            }

            int[] rent = new int[5];
            for (int j = 0; j < 5; j++)
            {
                rent[j] = 0; // Default to 0 if parsing fails
                if (fields.Length > 8 + j && !string.IsNullOrWhiteSpace(fields[8 + j]))
                {
                    if (!int.TryParse(fields[8 + j], out rent[j]))
                    {
                        Debug.LogWarning($"Invalid rent value for property '{name}' at level {j}, defaulting to 0.");
                    }
                }
            }

            // Create and add the property
            Housing property = new Housing(name, group, canBeBought, price, rent);
            Properties.Add(property);
        }
    }

    void AssignWaypointsToProperties()
    {
        if (Waypoints.Count != Properties.Count)
        {
            Debug.LogError("Mismatch between number of waypoints and properties!");
            return;
        }

        for (int i = 0; i < Properties.Count; i++)
        {
            Housing property = Properties[i];
            Transform waypoint = Waypoints[i];

            // Assign the waypoint to the property
            property.Waypoint = waypoint;

            // Add a collider and interaction script to the waypoint
            if (waypoint.GetComponent<Collider>() == null)
            {
                waypoint.gameObject.AddComponent<BoxCollider>();
            }

            if (waypoint.GetComponent<HouseInteraction>() == null)
            {
                waypoint.gameObject.AddComponent<HouseInteraction>().Initialize(property);
            }
        }
    }

    public void PrintProperties()
    {
        Debug.Log("Properties loaded from CSV:");
        foreach (var property in Properties)
        {
            Debug.Log($"Name: {property.Name}, Group: {property.Group}, Price: {property.Price}, Rent[0]: {property.Rent[0]}");
        }
    }

    public bool BuyProperty(Housing property, Player player)
    {
        if (property == null || player == null)
        {
            Debug.LogError("Invalid property or player!");
            return false;
        }

        if (!property.CanBeBought)
        {
            Debug.Log($"Property {property.Name} cannot be bought.");
            return false;
        }

        if (player.Money < property.Price)
        {
            Debug.Log($"Player {player.TokenName} does not have enough money to buy {property.Name}.");
            return false;
        }

        // Deduct money from the player and assign the property
        player.AdjustMoney(-property.Price); // Use AdjustMoney to update the player's money
        property.Owner = player;

        Debug.Log($"Player {player.TokenName} bought {property.Name} for {property.Price}.");
        return true;
    }
}
