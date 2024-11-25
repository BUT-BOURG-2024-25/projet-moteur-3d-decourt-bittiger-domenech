using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float speed = 5f; // Vitesse de d�placement de la cam�ra

    // Start is called before the first frame update
    void Start()
    {
        // Tu peux initialiser quelque chose ici si n�cessaire
    }

    // Update is called once per frame
    void Update()
    {
        // D�placer la cam�ra en avant selon son orientation actuelle (localement)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }
}
