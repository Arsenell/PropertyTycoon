using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TokenButtonSelect : MonoBehaviour
{
        [Header("UI References")]
    public TMP_Dropdown playerDropdown; // Assign in Inspector
    public string nextSceneName = "SampleScene";
    
    public TokenSelectManager tsm; // Set the next scene name

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnMouseDown()
    {
        OnPlayClicked();
        }
    
        private bool SceneExistsInBuildSettings(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                if (sceneNameFromPath == sceneName)
                {
                    return true;
                }
            }
            return false;
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

               // This method is called when the play button is clicked
        Debug.Log("Play button clicked!");
        tsm = FindObjectOfType<TokenSelectManager>();
        tsm.ConfirmSelection();
        // SceneManager.LoadScene("SampleScene"); // start the game scene
                }
    }



