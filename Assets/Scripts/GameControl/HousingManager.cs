using UnityEngine;
using System.Collections.Generic;

public class HousingManager : MonoBehaviour
{
    public List<Housing> Properties = new List<Housing>();
    public List<Transform> Waypoints = new List<Transform>();

    void Start()
    {
        PopulateWaypoints();
        LoadPropertiesFromCSV("DataForPTycoon");
        Debug.Log($"Waypoints count: {Waypoints.Count}");
        Debug.Log($"Properties count: {Properties.Count}");

        PrintLoadedProperties(); // Print all loaded properties for debugging

        AssignWaypointsToProperties();
    }

    public Housing GetPropertyAtWaypoint(int waypointIndex)
    {
        // Use the correct variable name: Properties
        foreach (Housing property in Properties)
        {
            if (property.WaypointIndex == waypointIndex)
            {
                return property;
            }
        }
        return null; // No property found at this waypoint
    }

    void PopulateWaypoints()
    {
        GameObject waypointsParent = GameObject.Find("Waypoints");
        if (waypointsParent == null)
        {
            Debug.LogError("Waypoints GameObject not found in the scene!");
            return;
        }

        Waypoints.Clear();
        foreach (Transform child in waypointsParent.transform)
        {
            Waypoints.Add(child);
        }

        Debug.Log($"Populated {Waypoints.Count} waypoints from the Waypoints GameObject.");
    }

    void LoadPropertiesFromCSV(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            Debug.LogError($"CSV file '{fileName}' not found in Resources folder!");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        Debug.Log($"Loaded {lines.Length} lines from CSV file.");

        for (int i = 1; i < lines.Length; i++) // Skip the header row
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                Debug.LogWarning($"Skipping empty line at index {i}.");
                continue;
            }

            string[] fields = lines[i].Split(',');

            // Skip invalid rows or comments
            if (fields.Length < 10 || fields[0].StartsWith("Notes") || fields[0].StartsWith("\""))
            {
                Debug.LogWarning($"Skipping invalid or comment row at index {i}: {lines[i]}");
                continue;
            }

            // Skip rows with unnamed properties or header-like values
            if (string.IsNullOrWhiteSpace(fields[1]) || fields[1].Trim() == "Space/property")
            {
                Debug.LogWarning($"Skipping invalid property at index {i}: {lines[i]}");
                continue;
            }

            // Parse property data
            string name = fields[1].Trim();
            string group = string.IsNullOrWhiteSpace(fields[3]) ? "No Group" : fields[3].Trim();
            bool canBeBought = fields[5].Trim().ToLower() == "yes";

            int price = 0;
            if (!int.TryParse(fields[7], out price))
            {
                Debug.LogWarning($"Invalid price for property '{name}' at index {i}, defaulting to 0.");
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
            Debug.Log($"Added property: {property.Name}, Group: {property.Group}, Price: {property.Price}, CanBeBought: {property.CanBeBought}");
        }

        Debug.Log($"Total properties loaded: {Properties.Count}");
    }

    void AssignWaypointsToProperties()
    {
        if (Waypoints.Count != Properties.Count)
        {
            Debug.LogWarning($"Mismatch between number of waypoints ({Waypoints.Count}) and properties ({Properties.Count}).");

            int count = Mathf.Min(Waypoints.Count, Properties.Count);

            for (int i = 0; i < count; i++)
            {
                AssignWaypointToProperty(Properties[i], Waypoints[i]);
            }

            if (Waypoints.Count > Properties.Count)
            {
                Debug.LogWarning($"Extra waypoints detected: {Waypoints.Count - Properties.Count} waypoints will not be assigned.");
            }
            else if (Properties.Count > Waypoints.Count)
            {
                Debug.LogWarning($"Extra properties detected: {Properties.Count - Waypoints.Count} properties will not have waypoints assigned.");
            }

            return;
        }

        for (int i = 0; i < Properties.Count; i++)
        {
            AssignWaypointToProperty(Properties[i], Waypoints[i]);
        }
    }

    void AssignWaypointToProperty(Housing property, Transform waypoint)
    {
        property.Waypoint = waypoint;
        property.WaypointIndex = Waypoints.IndexOf(waypoint); // Set the WaypointIndex

        if (waypoint.GetComponent<Collider>() == null)
        {
            waypoint.gameObject.AddComponent<BoxCollider>();
        }

        if (waypoint.GetComponent<HouseInteraction>() == null)
        {
            waypoint.gameObject.AddComponent<HouseInteraction>().Initialize(property);
        }

        Debug.Log($"Assigned waypoint '{waypoint.name}' to property '{property.Name}' with index {property.WaypointIndex}.");
    }

    public bool BuyProperty(Housing property, GamePlayer player)
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

        // Add the property to the player's owned properties
        player.AddProperty(property);

        Debug.Log($"Player {player.TokenName} bought {property.Name} for {property.Price}.");
        return true;
    }

    void PrintLoadedProperties()
    {
        Debug.Log("=== Loaded Properties ===");
        for (int i = 0; i < Properties.Count; i++)
        {
            Housing property = Properties[i];
            Debug.Log($"Index: {i}, Name: {property.Name}, Group: {property.Group}, CanBeBought: {property.CanBeBought}, Price: {property.Price}");
        }
        Debug.Log("=========================");
    }
}

