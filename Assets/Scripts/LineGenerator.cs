using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject solPrefab; // Le prefab de la plateforme
    public GameObject trapPrefab; // Le prefab des pièges
    public float segmentLength = 20f; // Longueur de chaque segment
    public int numberOfSegments = 7; // Nombre de segments visibles
    public Transform groupSoldat; // Référence au groupe de soldats

    private List<GameObject> segments; // Liste des segments de plateforme
    private Transform cameraTransform; // Référence à la caméra principale
    private List<PortalManager> portalManagers; // Liste des gestionnaires de portails
    private int spawnToggleCounter = 0; // Compteur pour alterner le spawn des ennemis et des pièges
    private int segmentIndex = 0; // Compteur global des segments

    void Start()
    {
        Debug.Log("LineGenerator Start");

        cameraTransform = Camera.main.transform; // Récupérer la caméra principale
        segments = new List<GameObject>();
        portalManagers = new List<PortalManager>();

        // Créer les segments initiaux
        for (int i = 0; i < numberOfSegments; i++)
        {
            Vector3 spawnPosition = new Vector3(0, 0, i * segmentLength);
            GameObject newSegment = Instantiate(solPrefab, spawnPosition, Quaternion.identity);
            segments.Add(newSegment);

            PortalManager portalManager = newSegment.GetComponent<PortalManager>();
            if (portalManager != null)
            {
                portalManagers.Add(portalManager);

                // Générer les portails
                portalManager.SpawnPortals(newSegment.transform.position);

                // Générer des pièges ou des ennemis
                if (segmentIndex >= 2)
                {
                    if (spawnToggleCounter % 4 == 0) // Rien
                    {
                        // Pas d'action
                    }
                    else if (spawnToggleCounter % 4 == 1) // Ennemis
                    {
                        portalManager.SpawnEnemies();
                    }
                    else if (spawnToggleCounter % 4 == 3) // Pièges
                    {
                        portalManager.SpawnTrap(trapPrefab);
                    }
                }

                spawnToggleCounter++;
            }
            segmentIndex++;
        }
    }

    public void Initialize()
    {
        Debug.Log("LineGenerator Initialize");

        cameraTransform = Camera.main.transform; // Assurez-vous que la caméra est bien attribuée
        segments = new List<GameObject>();
        portalManagers = new List<PortalManager>();

        // Recréez la ligne de départ
        for (int i = 0; i < numberOfSegments; i++)
        {
            Vector3 spawnPosition = new Vector3(0, 0, i * segmentLength);
            GameObject newSegment = Instantiate(solPrefab, spawnPosition, Quaternion.identity);
            segments.Add(newSegment);

            PortalManager portalManager = newSegment.GetComponent<PortalManager>();
            if (portalManager != null)
            {
                portalManagers.Add(portalManager);
                portalManager.SpawnPortals(newSegment.transform.position);

                // Générer des pièges ou des ennemis
                if (segmentIndex >= 2)
                {
                    if (spawnToggleCounter % 4 == 1) // Ennemis
                    {
                        portalManager.SpawnEnemies();
                    }
                    
                    else if (spawnToggleCounter % 4 == 3) // Pièges
                    {
                        portalManager.SpawnTrap(trapPrefab);
                    }
                }

                spawnToggleCounter++;
            }
            segmentIndex++;
        }
    }

    void Update()
    {
        MoveCamera();
        ManageLine();
    }

    void MoveCamera()
    {
        if (groupSoldat != null)
        {
            Vector3 cameraPosition = cameraTransform.position;
            cameraPosition.z = groupSoldat.position.z - 10f; // Garde une distance de 10 unités derrière
            cameraTransform.position = cameraPosition;
        }
        else
        {
            // Déplace la caméra automatiquement si le groupe n'est pas assigné
            cameraTransform.Translate(Vector3.forward * 12f * Time.deltaTime);
        }
    }

    void ManageLine()
    {
        // Gérer les segments en fonction de la position de la caméra
        for (int i = 0; i < segments.Count; i++)
        {
            GameObject segment = segments[i];

            // Supprimer les segments éloignés de la caméra
            if (segment.transform.position.z + segmentLength < cameraTransform.position.z)
            {
                PortalManager portalManager = portalManagers[i];
                if (portalManager != null)
                {
                    portalManager.DestroyPortalsAndEnemies(); // Détruit les portails, ennemis et pièges
                }

                Destroy(segment);
                segments.RemoveAt(i);
                portalManagers.RemoveAt(i);
                i--;
            }
        }

        // Ajouter un nouveau segment si nécessaire
        if (segments.Count < numberOfSegments)
        {
            Vector3 newPosition = segments[segments.Count - 1].transform.position + new Vector3(0, 0, segmentLength);
            GameObject newSegment = Instantiate(solPrefab, newPosition, Quaternion.identity);
            segments.Add(newSegment);

            PortalManager portalManager = newSegment.GetComponent<PortalManager>();
            if (portalManager != null)
            {
                portalManagers.Add(portalManager);

                // Générer les portails
                portalManager.SpawnPortals(newSegment.transform.position);

                // Générer des pièges ou des ennemis
                if (segmentIndex >= 2)
                {
                    if (spawnToggleCounter % 4 == 1) // Ennemis
                    {
                        portalManager.SpawnEnemies();
                    }
                    
                    else if (spawnToggleCounter % 4 == 3) // Pièges
                    {
                        portalManager.SpawnTrap(trapPrefab);
                    }
                }

                spawnToggleCounter++;
            }
            segmentIndex++;
        }
    }
}
