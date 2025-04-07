using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayButton : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown playerDropdown; // Assign in Inspector
    public string nextSceneName = "TokenSelectScreen"; // Set the next scene name

    private void OnMouseDown()
    {
        OnPlayClicked();
    }

    public void OnPlayClicked()
    {
        Debug.Log("Play button clicked!");

        // Check if the scene exists in the build settings
        if (!SceneExistsInBuildSettings(nextSceneName))
        {
            Debug.LogError($"Scene '{nextSceneName}' is not added to the Build Settings!");
            return;
        }

        // Find the MenuManager in the scene
        MenuManager menuManager = FindObjectOfType<MenuManager>();
        if (menuManager == null)
        {
            Debug.LogError("MenuManager not found in the scene!");
            return;
        }

        // Start the game
        menuManager.StartGame();
    }

    private bool SceneExistsInBuildSettings(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneFileName == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}