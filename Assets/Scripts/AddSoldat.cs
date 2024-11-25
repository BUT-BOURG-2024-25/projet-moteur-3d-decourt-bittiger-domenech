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
            // R�cup�rer l'action depuis le portail
            string action = other.gameObject.GetComponent<Portal>().getPortalAction();

            // D�couper la cha�ne en deux parties : symbole et valeur
            string[] actions = action.Split(',');

            if (actions.Length == 2)
            {
                string portalAction = actions[0];
                int portalValue;

                // Tenter de convertir le deuxi�me �l�ment en entier
                if (int.TryParse(actions[1], out portalValue))
                {
                    // Appeler la m�thode de spawn avec les arguments
                    SpawnObjectNearPlayer(portalAction, portalValue);
                }
                else
                {
                    Debug.LogError($"Erreur : Impossible de convertir '{actions[1]}' en entier.");
                }
            }
            else
            {
                Debug.LogError("Erreur : La cha�ne retourn�e par getPortalAction() n'a pas le bon format !");
            }
        }
    }
    public void SpawnObjectNearPlayer(string calcule, int valeurPortail)
    {
        Vector3 spawnPosition = Vector3.zero;
        int nbSoldat = countSoldat.Instance.getNombreSoldat();

        if (calcule == "+")
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
        }
        else if (calcule == "x")
        {
            int nbSoldatCible = nbSoldat * valeurPortail;
            nbSoldatCible = nbSoldatCible - nbSoldat;
            for (int i = 0; i < nbSoldatCible; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
                randomDirection.y = 0;
                spawnPosition = playerTransform.position + randomDirection;
                spawnPosition.y = playerTransform.position.y + 0.5f;

                GameObject newObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                newObject.transform.SetParent(playerTransform);
            }
        }
    }
}
