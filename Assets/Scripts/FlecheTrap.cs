using UnityEngine;

public class FlecheTrap : MonoBehaviour
{
    public GameObject arrowPrefab;
    public int rows = 3;
    public int cols = 3;
    public float arrowSpacing = 1f;
    public float speed = 6f; 

    public float startY = -2f; 
    public float maxY = 10f;
    public float minY = -5f;

    private GameObject[] arrows; 
    private bool trapActive = false;

    void Start()
    {
        SpawnArrows(); 
    }

    void Update()
    {
        if (trapActive)
        {
            MoveArrows(); 
        }
    }

    // Générer les flèches en groupe
    void SpawnArrows()
    {
        arrows = new GameObject[rows * cols];
        int index = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 position = new Vector3(
                    transform.position.x + i * arrowSpacing,
                    startY,
                    transform.position.z + j * arrowSpacing
                );

                GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.identity);
                arrow.SetActive(false); 
                arrows[index++] = arrow;

                FlecheBehavior arrowBehavior = arrow.AddComponent<FlecheBehavior>();
                arrowBehavior.OnArrowHit = OnArrowHit;
            }
        }

        ActivateArrows();
    }

    // Active les flèches et démarre leur mouvement
    void ActivateArrows()
    {
        foreach (GameObject arrow in arrows)
        {
            arrow.transform.position = new Vector3(
                arrow.transform.position.x,
                startY, 
                arrow.transform.position.z
            );
            arrow.SetActive(true);
        }

        trapActive = true;
    }

    // Déplacer les flèches en haut et en bas
    void MoveArrows()
    {
        bool allDestroyed = true;
        foreach (GameObject arrow in arrows)
        {
            if (arrow != null)
            {
                Vector3 newPosition = arrow.transform.position;
                newPosition.y += speed * Time.deltaTime;

                if (newPosition.y > maxY) 
                {
                    speed = -Mathf.Abs(speed); 
                }
                else if (newPosition.y < minY) 
                {
                    Destroy(arrow); 
                }
                else
                {
                    arrow.transform.position = newPosition;
                    allDestroyed = false; 
                }
            }
        }

        if (allDestroyed)
        {
            trapActive = false;
            SpawnArrows();
        }
    }

    void OnArrowHit(GameObject arrow, GameObject other)
    {
        if (other.CompareTag("Ally"))
        {
            Destroy(other);
        }
    }
}