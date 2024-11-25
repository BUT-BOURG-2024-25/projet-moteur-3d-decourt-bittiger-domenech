using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject solPrefab; // Le pr�fab du sol
    public GameObject soldat; // Le pr�fab du group de soldat
    public float segmentLength = 20f; // Longueur de chaque segment
    public float spawnDistance = 10f; // Distance apr�s laquelle un nouveau segment est g�n�r�
    public float deleteDistance = 10f; // Distance avant laquelle les segments sont supprim�s (plus t�t qu'avant)
    public int numberOfSegments = 7; // Nombre de segments visibles � tout moment

    private List<GameObject> segments; // Liste des segments de sol
    private Transform cameraTransform; // La cam�ra
    private List<PortalManager> portalManagers; // Liste des gestionnaires de portails

    void Start()
    {
        cameraTransform = Camera.main.transform; // R�cup�re la cam�ra principale
        segments = new List<GameObject>();
        portalManagers = new List<PortalManager>(); // Liste des gestionnaires de portails

        // Cr�er les segments de sol initiaux
        for (int i = 0; i < numberOfSegments; i++)
        {
            Vector3 spawnPosition = new Vector3(0, 0, i * segmentLength); // Position initiale des segments
            GameObject newSegment = Instantiate(solPrefab, spawnPosition, Quaternion.identity);
            segments.Add(newSegment);

            // Ajouter le gestionnaire de portails du segment
            PortalManager portalManager = newSegment.GetComponent<PortalManager>();
            if (portalManager != null)
            {
                portalManagers.Add(portalManager);

                // G�n�rer les portails pour ce segment
                Vector3 portalPosition1 = newSegment.transform.position + new Vector3(-1.1f, 0.5f, 0);
                Vector3 portalPosition2 = newSegment.transform.position + new Vector3(1.1f, 0.5f, 0);
                portalManager.SpawnPortal(portalPosition1);
                portalManager.SpawnPortal(portalPosition2);
            }
        }
    }

    void Update()
    {
        MoveCamera();
        ManageLine();
    }

    void MoveCamera()
    {
        // D�placer la cam�ra
        cameraTransform.Translate(Vector3.forward * 10f * Time.deltaTime); // 5f est la vitesse de la cam�ra
    }

    void ManageLine()
    {
        // G�rer les segments de sol en fonction de la position de la cam�ra
        for (int i = 0; i < segments.Count; i++)
        {
            GameObject segment = segments[i];

            // Si la cam�ra est � une certaine distance avant le segment, on le supprime
            if (segment.transform.position.z + segmentLength < cameraTransform.position.z - deleteDistance)
            {
                // Supprimer les portails associ�s au segment
                PortalManager portalManager = portalManagers[i];
                if (portalManager != null)
                {
                    portalManager.DestroyPortals(); // Appelle la m�thode pour supprimer les portails
                }

                // Supprimer le segment
                Destroy(segment);
                segments.RemoveAt(i);
                portalManagers.RemoveAt(i); // Retirer le gestionnaire de portails
                i--; // D�calage de l'indice pour �viter de sauter un segment apr�s suppression
            }
        }

        // Ajouter un nouveau segment si n�cessaire
        if (segments.Count < numberOfSegments)
        {
            // Assurer que la position du prochain segment soit correctement calcul�e
            Vector3 newPosition = segments[segments.Count - 1].transform.position + new Vector3(0, 0, segmentLength);
            GameObject newSegment = Instantiate(solPrefab, newPosition, Quaternion.identity);
            segments.Add(newSegment);

            // Ajouter le gestionnaire de portails pour ce segment
            PortalManager portalManager = newSegment.GetComponent<PortalManager>();
            if (portalManager != null)
            {
                portalManagers.Add(portalManager);

                // G�n�rer les portails pour ce nouveau segment
                Vector3 portalPosition1 = newSegment.transform.position + new Vector3(-1.1f, 0.5f, 0);
                Vector3 portalPosition2 = newSegment.transform.position + new Vector3(1.1f, 0.5f, 0);
                portalManager.SpawnPortal(portalPosition1);
                portalManager.SpawnPortal(portalPosition2);
            }
        }
    }
}
