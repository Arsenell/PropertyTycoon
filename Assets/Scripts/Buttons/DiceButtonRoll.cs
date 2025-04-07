using UnityEngine;
using System.Collections; // Add this to use IEnumerator

public class DiceButtonRoll : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite pressedSprite; // Assign the sprite to display when the button is pressed in the Inspector
    public Sprite defaultSprite; // Assign the default sprite in the Inspector
    public Dice dice; // Assign this in the Inspector
    public Dice Dice2; // Assign this in the Inspector

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if (spriteRenderer != null && pressedSprite != null)
        {
            spriteRenderer.sprite = pressedSprite; // Change to the pressed sprite
            if (dice != null && Dice2 != null)
            {
                StartCoroutine(RollAndMove());
            }
            else
            {
                Debug.LogError("Dice references are not assigned!");
            }
        }
    }

    private IEnumerator RollAndMove()
    {
        // Roll both dice
        dice.diceOnMouseDown();
        Dice2.diceOnMouseDown();

        // Wait for both dice to finish rolling
        while (!dice.IsRollComplete() || !Dice2.IsRollComplete())
        {
            yield return null; // Wait for the next frame
        }

        // Get the rolled values
        int tmpVal1 = dice.GetRolledValue();
        int tmpVal2 = Dice2.GetRolledValue();

        // Check if the player rolled a double
        bool isDouble = tmpVal1 == tmpVal2;

        // Move the current player
        int currentPlayerIndex = GameControl.Instance.GetCurrentPlayerIndex(); // Get the current player's index
        GameControl.Instance.MovePlayer(currentPlayerIndex + 1, tmpVal1 + tmpVal2, isDouble); // Pass the correct player index (1-based)
    }

    void OnMouseUp()
    {
        if (spriteRenderer != null && defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite; // Revert to the default sprite
        }
    }
}