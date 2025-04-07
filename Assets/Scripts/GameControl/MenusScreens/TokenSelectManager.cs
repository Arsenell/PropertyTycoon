using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TokenSelectManager : MonoBehaviour
{
    public GameObject[] playerPanels; // Drag Player1SelectPanel, Player2..., etc.
    public Image[] spriteSelectors; // Drag the Image components for each player's sprite selection

    public void Start()
    {
        int count = GameSettings.playerCount;

        // Hide or show panels based on the selected player count
        for (int i = 0; i < playerPanels.Length; i++)
        {
            playerPanels[i].SetActive(i < count); // Show if index < selected count
        }
    }

    public void ConfirmSelection()
    {
        // Store the selected sprites in GameSettings
        for (int i = 0; i < GameSettings.playerCount; i++)
        {
            GameSettings.playerSprites[i] = spriteSelectors[i].sprite;
        }

        Debug.Log("Player sprites have been saved.");
        SceneManager.LoadScene("SampleScene"); // start the game scene
    }
}