using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class AddSoldat : MonoBehaviour
{
    [SerializeField]
    private LayerMask GroundLayer;

    [SerializeField]
    private GameObject objectToSpawn;

    [SerializeField]
    private float spawnRadius = 2.0f;

    [SerializeField]
    private float checkRadius = 1.0f;

    [SerializeField]
    private int maxAttempts = 10;

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Portal"))
        {
            // Récupérer l'action depuis le portail
            string action = other.gameObject.GetComponent<Portal>().getPortalAction();

            // Découper la chaîne en deux parties : symbole et valeur
            string[] actions = action.Split(',');

            if (actions.Length == 2)
            {
                string portalAction = actions[0];
                int portalValue;

                // Tenter de convertir le deuxième élément en entier
                if (int.TryParse(actions[1], out portalValue))
                {
                    // Appeler la méthode de spawn avec les arguments
                    SpawnObjectNearPlayer(portalAction, portalValue);
                }
                else
                {
                    Debug.LogError($"Erreur : Impossible de convertir '{actions[1]}' en entier.");
                }
            }
            else
            {
                Debug.LogError("Erreur : La chaîne retournée par getPortalAction() n'a pas le bon format !");
            }
        }
    }

    public void SpawnObjectNearPlayer(string calcule, int valeurPortail)
    {
        if (objectToSpawn == null)
        {
            Debug.LogWarning("objectToSpawn est null, tentative de rechargement...");
            objectToSpawn = Resources.Load<GameObject>("Soldat");
            if (objectToSpawn == null)
            {
                Debug.LogError("Impossible de charger le prefab Soldat !");
                return;
            }
        }



        Vector3 spawnPosition = Vector3.zero;
        int nbSoldat = countSoldat.Instance.getNombreSoldat();
        //Debug.Log($"[DEBUG] calcule={calcule}, valeurPortail={valeurPortail}, nbSoldat={nbSoldat}, objectToSpawn={objectToSpawn}");

        if (calcule == "+")
        {
            if (nbSoldat + valeurPortail <= 200)
            {
                for (int i = 0; i < valeurPortail; i++)
                {
                    Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
                    randomDirection.y = 0;
                    spawnPosition = playerTransform.position + randomDirection;
                    spawnPosition.y = playerTransform.position.y + 0.5f;

                    GameObject newObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                    newObject.transform.SetParent(playerTransform);
                }
                //Debug.Log($"{valeurPortail} soldats ajoutés.");
            }
        }
        else if (calcule == "x")
        {
            int nbSoldatCible = nbSoldat * valeurPortail;

            int nbToAdd = 0;
            if (nbSoldatCible > 200)
            {
                nbToAdd = 0;
            }
            else
            {
                nbToAdd = nbSoldatCible - nbSoldat;
            }

            for (int i = 0; i < nbToAdd; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
                randomDirection.y = 0;
                spawnPosition = playerTransform.position + randomDirection;
                spawnPosition.y = playerTransform.position.y + 0.5f;

                GameObject newObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                newObject.transform.SetParent(playerTransform);
            }
            //Debug.Log($"{nbToAdd} soldats ajoutés.");
        }
        else if (calcule == "-")
        {
            int nbSoldatToRemove = Mathf.Min(valeurPortail, nbSoldat);
            int removedCount = 0;
            foreach (Transform child in playerTransform)
            {
                if (child.CompareTag("Soldat"))
                {
                    //Debug.Log($"Destruction du soldat : {child.name}");
                    Destroy(child.gameObject);
                    removedCount++;
                    if (removedCount >= nbSoldatToRemove)
                    {
                        break;
                    }
                }
            }
            //Debug.Log($"{nbSoldatToRemove} soldats retirés.");
        }
    }


}