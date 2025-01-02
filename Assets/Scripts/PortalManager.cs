using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public GameObject portalPrefab; // Le prefab du portail
    public GameObject enemyPrefab; // Le prefab de l'ennemi
    public float portalSpacing = 2f; // Espacement entre les portails
    public int numberOfPortals = 2; // Nombre de portails générés
    public int enemyCount = 3; // Nombre d'ennemis à spawner
    public int maxEnemies = 100; // Limite maximale d'ennemis (facultatif)
    public int enemyIncrement = 1; // Nombre d'ennemis supplémentaires à chaque spawn


    private List<GameObject> portals = new List<GameObject>();
    private GameObject enemyGroupObject;
    private GameObject trapObject;


    public void SpawnPortals(Vector3 platformPosition)
    {
        // Génére les portails sur la plateforme
        for (int i = 0; i < numberOfPortals; i++)
        {
            Vector3 portalPosition = platformPosition + new Vector3((i - 0.5f) * portalSpacing, 0.5f, 0);
            GameObject newPortal = Instantiate(portalPrefab, portalPosition, Quaternion.identity);
            portals.Add(newPortal);
        }
    }

    public void SpawnTrap(GameObject trapPrefab)
    {
        if (trapPrefab == null)
        {
            Debug.LogError("Le prefab du piège (trapPrefab) n'est pas assigné !");
            return;
        }

        trapObject = Instantiate(trapPrefab, transform.position, Quaternion.identity);
    }

      public void SpawnEnemies()
    {
        if (portals.Count == 0) return;

        GameObject enemyGroupObject = new GameObject("EnemyGroupe");
        enemyGroupObject.transform.position = transform.position;

        EnemyGroup enemyGroupScript = enemyGroupObject.AddComponent<EnemyGroup>();

        for (int i = 0; i < enemyCount; i++)
        {
            float horizontalSpacing = 1.5f;
            float verticalSpacing = 1.0f;

            int row = i / 3;
            int column = i % 3;
            float offsetX = (column - 1) * horizontalSpacing;
            float offsetZ = row * verticalSpacing;

            Vector3 spawnPosition = transform.position + new Vector3(offsetX, 0, offsetZ);

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.transform.parent = enemyGroupObject.transform;

            enemyGroupScript.AddEnemy(enemy);
        }

        enemyCount = Mathf.Min(enemyCount + enemyIncrement, maxEnemies);
    }
    public void DestroyPortalsAndEnemies()
    {
        foreach (GameObject portal in portals)
        {
            Destroy(portal);
        }
        portals.Clear();

        if (enemyGroupObject != null)
        {
            Destroy(enemyGroupObject);
            enemyGroupObject = null;
        }
        
        if (trapObject != null)
        {
            FlecheTrap flecheTrap = trapObject.GetComponent<FlecheTrap>();
            if (flecheTrap != null)
            {
                flecheTrap.DestroyArrows();
            }

            Destroy(trapObject);
        }
    }
}
