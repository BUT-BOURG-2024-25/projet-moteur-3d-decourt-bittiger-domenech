using UnityEngine;

public class Ally : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                //enemy.Die();
            }
        }
    }

    public void Die()
    {
        AllyGroup group = GetComponentInParent<AllyGroup>();
        if (group != null)
        {
            group.RemoveAlly(gameObject);
        }
        Destroy(gameObject);
    }
}
