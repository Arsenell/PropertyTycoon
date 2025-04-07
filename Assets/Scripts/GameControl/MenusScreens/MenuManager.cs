using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TMP_Dropdown playerDropdown;

    public void StartGame()
    {
        // Dropdown index 0 = 2 players, 1 = 3, etc.
        GameSettings.playerCount = playerDropdown.value + 2;

        // Initialize the playerSprites array with the correct size
        GameSettings.playerSprites = new Sprite[GameSettings.playerCount];

        // Go to token selection or game scene
        SceneManager.LoadScene("TokenSelectScreen");
    }

}
