using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSoldat : MonoBehaviour
{
    public GameObject soldatGroup; // Le prefab du groupe de soldats
    public float distanceFromCamera = 10f; // Distance devant la cam�ra pour faire appara�tre le groupe

    // Spawn le groupe de soldats
    public void SpawnSoldatGroup(Vector3 position)
    {
        Instantiate(soldatGroup, position, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Obtenir la position de la cam�ra
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            // Calculer la position devant la cam�ra
            Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;

            // Forcer la hauteur (y) � une valeur fixe
            spawnPosition.y = 0.5f; // Exemple : 0 pour le sol

            // Appeler la fonction pour spawn le groupe de soldats
            SpawnSoldatGroup(spawnPosition);
        }
        else
        {
            Debug.LogError("Aucune cam�ra principale n'a �t� trouv�e !");
        }
    }
}
