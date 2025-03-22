using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
    private bool rollComplete = false; // Flag to indicate if the roll is complete

    private Sprite[] diceSides;
    private SpriteRenderer rend; // get all the dice sides using the spirte renderer component
    private int whosTurn = 1; // whos tunturn it is 
    private bool coroutineAllowed = true; // to check if the coroutine is allowed to run or not
     private int rolledValue = 0;
private void Start()
{
    rend = GetComponent<SpriteRenderer>();
    diceSides = Resources.LoadAll<Sprite>("DiceSides/");
    Debug.Log("Number of dice sides loaded: " + diceSides.Length);

    if (diceSides.Length >= 6)
    {
        rend.sprite = diceSides[5]; // Set to the sixth sprite
    }
    else
    {
        Debug.LogError("Not enough dice sides found in Resources/DiceSides/. Expected at least 6.");
    }
}
 private void OnMouseDown()
{
    if (!GameControl.isGameOver && coroutineAllowed)
        StartCoroutine("RollTheDice");
}

    public bool IsRollComplete()
    {
        return rollComplete;
    }

  public int GetRolledValue()
    {
        return rolledValue; // Return the rolled dice value
    }

    public void diceOnMouseDown(){ //using a button to roll the dice so would need to call this function
        OnMouseDown();
    }

private IEnumerator RollTheDice()
{
    rollComplete = false; // Reset the flag
    coroutineAllowed = false;
    int randomDiceSide = 0;

    for (int i = 0; i <= 20; i++)
    {
        randomDiceSide = Random.Range(0, 6); // Random index for the dice face
        rend.sprite = diceSides[randomDiceSide]; // Update the sprite
        yield return new WaitForSeconds(0.05f);
    }

    rolledValue = randomDiceSide + 1; // Add 1 to match the dice face value
    Debug.Log("Dice Value: " + rolledValue);
    GameControl.diceSideThrown = rolledValue; // Update GameControl with the rolled value
    coroutineAllowed = true;
    rollComplete = true; // Set the flag to true
}
}
