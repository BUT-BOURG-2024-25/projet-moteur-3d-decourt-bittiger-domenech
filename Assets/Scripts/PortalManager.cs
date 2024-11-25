using TMPro;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public GameObject portalPrefab; // Le prefab du portail
    public Material redMaterial; // Mat�riau rouge
    public Material blueMaterial; // Mat�riau bleu
    public float portalSpacing = 2f; // Espacement entre les portails
    public int numberOfPortals = 2; // Nombre de portails � g�n�rer

    public void SpawnPortal(Vector3 position)
    {
        // Cr�e un nouveau portail
        GameObject newPortal = Instantiate(portalPrefab, position, Quaternion.identity);
        Material randomMaterial = null;

        // Change la couleur al�atoire
        Renderer portalRenderer = newPortal.GetComponent<Renderer>();
        if (portalRenderer != null)
        {
            randomMaterial = Random.value > 0.5f ? redMaterial : blueMaterial;
            portalRenderer.material = randomMaterial;
        }

        // Configure le texte
        TextMeshPro text = newPortal.GetComponentInChildren<TextMeshPro>();
        if (text != null)
        {
            // G�n�re un symbole et une valeur
            string[] symbols = { "-", "+", "x" };
            string randomSymbol;
            int randomValue;

            if (randomMaterial == redMaterial)
            {
                randomSymbol = symbols[0];
                randomValue = Random.Range(1, 11); // Valeur entre 1 et 10
            }
            else
            {
                randomSymbol = symbols[Random.Range(1, 3)];
                if (randomSymbol == "x")
                {
                    randomValue = Random.Range(1, 4); // Valeur entre 1 et 3
                }
                else
                {
                    randomValue = Random.Range(1, 11); // Valeur al�atoire de 1 � 10
                }
            }

            // Met � jour le texte
            text.text = $"{randomSymbol} {randomValue}";
        }
    }

    void Start()
    {
        // R�cup�re les informations de l'objet appelant
        Vector3 startPosition = transform.position; // Position de l'objet
        Vector3 size = GetComponent<Renderer>().bounds.size; // Taille de l'objet

        Vector3 portalPosition1 = startPosition + new Vector3(-1, 0.5f, 0);
        SpawnPortal(portalPosition1);

        Vector3 portalPosition2 = startPosition + new Vector3(1, 0.5f, 0);
        SpawnPortal(portalPosition2);
    }
}
