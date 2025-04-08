using UnityEngine;

public class Housing
{
    public string Name { get; set; }
    public string Group { get; set; }
    public bool CanBeBought { get; set; }
    public int Price { get; set; }
    public int[] Rent { get; set; }
    public GamePlayer Owner { get; set; } // Reference to the player who owns the property
    public Transform Waypoint { get; set; } // Reference to the waypoint Transform
    public int WaypointIndex { get; set; } // Index of the waypoint

    // Property to check if the property is owned
    public bool IsOwned
    {
        get { return Owner != null; }
    }

    public Housing(string name, string group, bool canBeBought, int price, int[] rent)
    {
        Name = name;
        Group = group;
        CanBeBought = canBeBought;
        Price = price;
        Rent = rent;
        Owner = null; // Initially, no one owns the property
        Waypoint = null;
        WaypointIndex = -1; // Default to -1 (unassigned)
    }

    public bool IsSpecialProperty()
    {
        // Define special properties that cannot be bought
        return Group == "Station" || Group == "Utilities" || Name == "Income Tax" || Name == "Go" || Name == "Jail" || Name == "Free Parking";
    }

    // Make this method public so it can be accessed from other classes
    public int GetRent(int houses)
    {
        if (houses < 0 || houses >= Rent.Length)
        {
            Debug.LogError($"Invalid number of houses: {houses}. Returning rent for 0 houses.");
            return Rent[0];
        }
        return Rent[houses];
    }
}