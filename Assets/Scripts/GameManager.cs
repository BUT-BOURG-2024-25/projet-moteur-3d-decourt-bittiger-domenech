using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject mainCameraPrefab;
    public GameObject directionalLightPrefab;
    public GameObject uiPrefab;
    public GameObject eventSystemPrefab;
    public GameObject lineManagerPrefab;

    private GameObject currentLineManager;
    private bool isGameOver = false;
    private int score = 0; // Le score actuel du joueur
    private int bestScore = 0; // Meilleur score sauvegard�
    public TMPro.TextMeshProUGUI scoreText; // Lien vers le texte du score
    public TMPro.TextMeshProUGUI bestScoreText; // Lien vers le texte du meilleur score (Game Over)
    public GameObject gameOverUI; // Lien vers l'�cran de Game Over

    private Coroutine scoreCoroutine;

    private void Awake()
    {
        // S'assurer qu'il n'y a qu'un seul GameManager
        GameManager[] managers = FindObjectsOfType<GameManager>();
        if (managers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Pr�serve le GameManager lors du changement de sc�ne
    }

    private void Start()
    {
        Debug.Log("D�marrage du jeu...");
        EnsureEssentialObjectsExist(); // V�rifie seulement, ne recr�e pas
        LoadBestScore(); // Charger le meilleur score
        InitializeGame();
    }

    private void InitializeGame()
    {
        isGameOver = false;
        score = 0;
        UpdateScoreUI(); // Met � jour l'affichage initial du score
        gameOverUI.SetActive(false); // Cache l'�cran de Game Over

        // D�marrer la routine d'incr�mentation du score
        if (scoreCoroutine != null)
            StopCoroutine(scoreCoroutine);
        scoreCoroutine = StartCoroutine(IncrementScore());
    }

    private IEnumerator IncrementScore()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(1f); // Ajouter 1 point chaque seconde
            AddScore(1);
        }
    }

    public void AddScore(int points)
    {
        if (!isGameOver)
        {
            score += points;
            Debug.Log($"Score ajout� : {points}, Score total : {score}");
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + score;
            Debug.Log($"UI mise � jour : Score : {score}");
        }
        else
        {
            Debug.LogError("scoreText n'est pas assign� dans le GameManager !");
        }
    }

    public void RestartGame()
    {
        Debug.Log("Red�marrage du jeu...");

        // Sauvegarder le meilleur score avant de r�initialiser
        SaveBestScore();

        // Supprimer tous les objets dans la sc�ne sauf le GameManager
        CleanUpScene();

        // R�initialiser la sc�ne en recr�ant les objets essentiels
        CreateEssentialObjects();

        // R�initialiser l'�tat du jeu
        InitializeGame();
    }

    private void EnsureEssentialObjectsExist()
    {
        Debug.Log("V�rification des objets essentiels existants...");

        // Cam�ra
        if (Camera.main == null && mainCameraPrefab != null)
        {
            Instantiate(mainCameraPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("Main Camera cr��e.");
        }

        // Lumi�re directionnelle
        if (FindObjectOfType<Light>() == null && directionalLightPrefab != null)
        {
            Instantiate(directionalLightPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("Directional Light cr��e.");
        }

        // UI
        if (FindObjectOfType<Canvas>() == null && uiPrefab != null)
        {
            Instantiate(uiPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("UI cr��e.");
        }

        // EventSystem
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null && eventSystemPrefab != null)
        {
            Instantiate(eventSystemPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("EventSystem cr��.");
        }

        // LineManager
        if (FindObjectOfType<LineGenerator>() == null && lineManagerPrefab != null)
        {
            currentLineManager = Instantiate(lineManagerPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("LineManager cr��.");
        }
    }

    private void CreateEssentialObjects()
    {
        Debug.Log("Cr�ation des objets essentiels...");

        // Cam�ra
        if (mainCameraPrefab != null)
        {
            Instantiate(mainCameraPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("Main Camera cr��e.");
        }

        // Lumi�re directionnelle
        if (directionalLightPrefab != null)
        {
            Instantiate(directionalLightPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("Directional Light cr��e.");
        }

        // UI
        if (uiPrefab != null)
        {
            Instantiate(uiPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("UI cr��e.");
        }

        // EventSystem
        if (eventSystemPrefab != null)
        {
            Instantiate(eventSystemPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("EventSystem cr��.");
        }

        // LineManager
        if (lineManagerPrefab != null)
        {
            currentLineManager = Instantiate(lineManagerPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("LineManager cr��.");
        }
    }

    private void CleanUpScene()
    {
        Debug.Log("Nettoyage de la sc�ne...");

        // Liste temporaire pour �viter les erreurs lors de la suppression
        List<GameObject> objectsToDestroy = new List<GameObject>();

        // Parcourir tous les objets actifs dans la sc�ne
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj != this.gameObject) // Ne pas supprimer le GameManager lui-m�me
            {
                objectsToDestroy.Add(obj);
            }
        }

        // Supprimer tous les objets enregistr�s
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        Debug.Log("Nettoyage termin�.");
    }

    public void GameOver()
    {
        if (isGameOver) return; // �viter d'appeler plusieurs fois GameOver

        Debug.Log("Game Over! Tous les soldats ont �t� �limin�s.");
        isGameOver = true;

        // Arr�ter la routine de score
        if (scoreCoroutine != null)
            StopCoroutine(scoreCoroutine);

        // Sauvegarder le meilleur score
        SaveBestScore();

        // Afficher l'�cran de fin de partie
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Mettre � jour le meilleur score dans l'UI Game Over
        if (bestScoreText != null)
        {
            bestScoreText.text = $"Meilleur Score : {bestScore}";
        }

        // Arr�ter le jeu (si n�cessaire)
        Time.timeScale = 0; // Mettre le jeu en pause
    }

    private void SaveBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
            Debug.Log($"Nouveau meilleur score sauvegard� : {bestScore}");
        }
    }

    private void LoadBestScore()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = PlayerPrefs.GetInt("BestScore");
            Debug.Log($"Meilleur score charg� : {bestScore}");
        }
    }
}
