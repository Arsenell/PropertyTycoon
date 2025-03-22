using UnityEngine;

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
        int tmpVal1 = 0;
        int tmpVal2 = 0;
        if (spriteRenderer != null && pressedSprite != null)
        {
            spriteRenderer.sprite = pressedSprite; // Change to the pressed sprite
            if (dice != null)
            {
                dice.diceOnMouseDown(); // Call the diceOnMouseDown method from the Dice script
                tmpVal1 = dice.GetRolledValue();
                Dice2.diceOnMouseDown();
                tmpVal2 = Dice2.GetRolledValue();
                GameControl.MovePlayer(1, tmpVal1 + tmpVal2);
            }
            else
            {
                Debug.LogError("Dice reference is not assigned!");
            }
        }
    }

    void OnMouseUp()
    {
        if (spriteRenderer != null && defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite; // Revert to the default sprite
        }
    }
}