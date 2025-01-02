using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public float groupSpeed = 5f; // Vitesse du groupe
    public List<GameObject> enemies = new List<GameObject>(); // Liste des ennemis dans ce groupe
    private Transform target; // Référence au groupeSoldat

    void Start()
    {
        // Trouver le groupeSoldat dans la scène
        GameObject targetObject = GameObject.FindWithTag("Player");
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
        else
        {
            Debug.LogError("Aucun GameObject avec le tag 'Player' trouvé !");
        }
    }

    void Update()
    {
        if (target == null) return;

        // Déplace le groupe vers le groupeSoldat
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * groupSpeed * Time.deltaTime;

        // S'assure que tous les ennemis suivent aussi cette position
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.UpdateTarget(target.position);
                }
            }
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy);
    }
}
