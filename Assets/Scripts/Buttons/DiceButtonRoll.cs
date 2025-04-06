using UnityEngine;
using System.Collections; // Add this to use IEnumerator
using GameControlNamespace; // Add this at the top of DiceButtonRoll.cs

namespace GameControlNamespace
{
    public class GameControl : MonoBehaviour
    {
        public static void MovePlayer(int playerToMove, int diceSideThrown)
        {
            // Implementation
        }
    }
}

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

        // Move the player
        GameControl.MovePlayer(1, tmpVal1 + tmpVal2);
    }

    void OnMouseUp()
    {
        if (spriteRenderer != null && defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite; // Revert to the default sprite
        }
    }
}