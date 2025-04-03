using UnityEngine;

public class Housing
{
    public string Name { get; private set; }
    public string Group { get; private set; }
    public bool CanBeBought { get; private set; }
    public int Price { get; private set; }
    public int[] Rent { get; private set; } // Rent for 0, 1, 2, 3, 4 houses and hotel
    public Player Owner { get; set; } // Tracks the owner of the property
    public Transform Waypoint { get; set; } // Reference to the associated waypoint

    public Housing(string name, string group, bool canBeBought, int price, int[] rent)
    {
        Name = name;
        Group = group;
        CanBeBought = canBeBought;
        Price = price;
        Rent = rent;
    }

    public int GetRent(int houses)
    {
        if (houses < 0 || houses >= Rent.Length)
        {
            Debug.LogError("Invalid number of houses!");
            return 0;
        }
        return Rent[houses];
    }
}