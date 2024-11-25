using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public GameObject portalPrefab; // Le prefab du portail
    public float portalSpacing = 2f; // Espacement entre les portails
    public int numberOfPortals = 2; // Nombre de portails � g�n�rer

    private List<GameObject> portals = new List<GameObject>(); // Liste des portails g�n�r�s pour ce segment

    public void SpawnPortal(Vector3 position)
    {
        // Cr�e un nouveau portail
        GameObject newPortal = Instantiate(portalPrefab, position, Quaternion.identity);
        Material randomMaterial = null;

        portals.Add(newPortal); // Ajoute le portail � la liste
    }

    public void DestroyPortals()
    {
        // D�truit tous les portails associ�s � ce segment de sol
        foreach (GameObject portal in portals)
        {
            Destroy(portal);
        }

        portals.Clear(); // Vide la liste apr�s suppression
    }
}
