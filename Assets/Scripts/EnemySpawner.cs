using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform enemyGroupParent; 
    public Vector3 spawnPosition = new Vector3(0, 0, 0); 
    public float spawnInterval = 5f; 
    private int enemyCount = 2;

    void Start()
    {
        StartCoroutine(SpawnEnemyGroups());
    }

    IEnumerator SpawnEnemyGroups()
    {
        while (true)
        {
            GameObject enemyGroup = new GameObject("EnemyGroup");
            enemyGroup.transform.position = spawnPosition;
            enemyGroup.transform.parent = enemyGroupParent;

            int row = 1; 
            int spawned = 0;

            while (spawned < enemyCount)
            {
                int enemiesInRow = Mathf.Min(row, enemyCount - spawned);

                for (int i = 0; i < enemiesInRow; i++)
                {
                    Vector3 enemyPosition = new Vector3(
                        (i - (enemiesInRow - 1) / 2f) * 2f, 
                        0,
                        -row * 2f
                    );

                    GameObject newEnemy = Instantiate(enemyPrefab, enemyGroup.transform);
                    newEnemy.transform.localPosition = enemyPosition;
                }

                spawned += enemiesInRow; 
                row++; 
            }

            enemyCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
