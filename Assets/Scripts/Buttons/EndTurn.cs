using UnityEngine;

public class EndTurn : MonoBehaviour
{
    private GameControl gameControl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the GameControl instance in the scene
        gameControl = GameControl.Instance;

        if (gameControl == null)
        {
            Debug.LogError("EndTurn: GameControl instance not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when the EndTurn button is clicked
    private void OnMouseDown()
    {
        if (gameControl != null && gameControl.IsTurnActive())
        {
            Debug.Log("EndTurn button clicked. Ending the current player's turn.");
            gameControl.EndTurn(); // Call a new method to handle ending the turn
        }
        else
        {
            Debug.LogWarning("EndTurn button clicked, but the turn is not active.");
        }
    }
}
