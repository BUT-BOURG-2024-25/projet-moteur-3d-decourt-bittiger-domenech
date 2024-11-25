using TMPro; 
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public GameObject portalPrefab; // Le prefab du portail
    public Material redMaterial; // Mat�riau rouge
    public Material blueMaterial; // Mat�riau bleu

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
                if (randomSymbol == "*")
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
        Vector3 startPosition = new Vector3(0, 0, 0);
        float distanceBetweenPortals = 2.2f;
        for (int i = 0; i < 2; i++)
        {
            Vector3 position = startPosition + new Vector3(i * distanceBetweenPortals, 0, 0);
            SpawnPortal(position);
        }
    }
}
