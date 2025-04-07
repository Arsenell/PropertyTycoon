using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Assign the player prefab in the Inspector
    public Transform[] spawnPoints; // Assign up to 6 spawn points in the Inspector

    void Start()
    {
        SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        int playerCount = GameSettings.playerCount;

        for (int i = 0; i < playerCount; i++)
        {
            if (spawnPoints[i] == null)
            {
                Debug.LogError($"Spawn point {i + 1} is not assigned!");
                continue;
            }

            GameObject player = Instantiate(playerPrefab, spawnPoints[i].position, Quaternion.identity);
            player.name = $"Player {i + 1}"; // Name the player GameObject for easier debugging

            GamePlayer gamePlayer = player.GetComponent<GamePlayer>();
            if (gamePlayer != null)
            {
                gamePlayer.SetSprite(GameSettings.playerSprites[i]);
                gamePlayer.TokenName = $"Player {i + 1}";
                gamePlayer.playerID = i + 1;
            }
            else
            {
                Debug.LogError($"GamePlayer component is missing on the player prefab!");
            }

            Debug.Log($"Spawned Player {i + 1} at {spawnPoints[i].position}");
        }
    }
}
