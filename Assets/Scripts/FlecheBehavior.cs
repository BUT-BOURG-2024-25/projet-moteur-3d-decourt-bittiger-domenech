using UnityEngine;

public class FlecheBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Soldat"))
        {
            // Détruire le Soldat touché
            Destroy(other.gameObject);
        }
    }
}
