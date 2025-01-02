using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class countSoldat : Singleton<countSoldat>
{
    [SerializeField]
    private TextMeshProUGUI soldatCountText;

    private int soldatCount;
    private GameManager gameManager; // Référence au GameManager

    void Start()
    {
        // Récupérer la référence du GameManager
        gameManager = GameObject.FindObjectOfType<GameManager>();

        CountSoldats();
    }

    void Update()
    {
        CountSoldats();

        // Si le nombre de soldats atteint 0, déclencher la fin de la partie
        if (getNombreSoldat() == 0)
        {
            if (gameManager != null)
            {
                gameManager.GameOver();
            }
        }
    }

    private void CountSoldats()
    {
        soldatCountText.text = getNombreSoldat().ToString();
    }

    public int getNombreSoldat()
    {
        soldatCount = 0;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Soldat"))
            {
                soldatCount++;
            }
        }
        return soldatCount;
    }
}
