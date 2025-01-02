
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Le prefab du joueur (groupSoldat)
    public float distanceFromCamera = 10f; // Distance devant la caméra pour faire apparaître le joueur
    private GameObject currentPlayerInstance; // Référence au joueur actuel

    // Méthode pour spawn le joueur
    public void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Le prefab du joueur (groupSoldat) n'est pas assigné !");
            return;
        }

        if (currentPlayerInstance != null)
        {
            Destroy(currentPlayerInstance); // Détruire le joueur précédent s'il existe
        }

        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Aucune caméra principale n'a été trouvée !");
            return;
        }

        // Position du spawn du joueur devant la caméra
        Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
        spawnPosition.y = 0.5f; // Fixe la hauteur pour ne pas être sous le sol

        // Instancier le joueur
        currentPlayerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        currentPlayerInstance.name = "Player"; // Renomme l'objet instancié
    }

    // Méthode à appeler au début de la partie ou après un redémarrage
    public void InitializePlayer()
    {
        SpawnPlayer();
    }
}