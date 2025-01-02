using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallowPlayer : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    private GameObject player;
    private Vector3 direction;
    private bool shouldMoveHorizontally = true;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // Trouver le joueur dans la scène
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMoveHorizontally)
        {
            direction = (player.transform.position - transform.position).normalized;
            Vector3 horizontalMove = new Vector3(direction.x, 0, direction.z) * speed;
            rb.velocity = new Vector3(horizontalMove.x, rb.velocity.y, horizontalMove.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Soldat"))
        {
            shouldMoveHorizontally = false;
            rb.velocity = new Vector3(0, rb.velocity.y, 0); // Arrêter le mouvement horizontal
        }
    }
}
