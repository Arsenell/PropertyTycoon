using UnityEngine;

public class Housing
{
    public string Name;
    public string Group;
    public bool CanBeBought;
    public int Price;
    private int[] Rent;
    public GamePlayer Owner;
    public Transform Waypoint;

    public Housing(string name, string group, bool canBeBought, int price, int[] rent)
    {
        Name = name;
        Group = group;
        CanBeBought = canBeBought;
        Price = price;
        Rent = rent;
        Owner = null;
        Waypoint = null;
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