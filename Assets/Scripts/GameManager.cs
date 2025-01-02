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
    private int bestScore = 0; // Meilleur score sauvegardé
    public TMPro.TextMeshProUGUI scoreText; // Lien vers le texte du score
    public TMPro.TextMeshProUGUI bestScoreText; // Lien vers le texte du meilleur score (Game Over)
    public GameObject gameOverUI; // Lien vers l'écran de Game Over
    public PlayerSpawner playerSpawner;

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

        DontDestroyOnLoad(gameObject); // Préserve le GameManager lors du changement de scène
    }

    private void Start()
    {
        Debug.Log("Démarrage du jeu...");
        EnsureEssentialObjectsExist(); // Vérifie seulement, ne recrée pas
        LoadBestScore(); // Charger le meilleur score
        InitializeGame();
    }

    private void InitializeGame()
    {

        Debug.Log("Réinitialisation de la partie...");

        if (playerSpawner != null)
        {
            playerSpawner.InitializePlayer();
        }
        else
        {
            Debug.LogError("PlayerSpawner non assigné dans le GameManager !");
        }

        isGameOver = false;
        score = 0;
        UpdateScoreUI(); // Met à jour l'affichage initial du score
        gameOverUI.SetActive(false); // Cache l'écran de Game Over

        // Démarrer la routine d'incrémentation du score
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
            //Debug.Log($"Score ajouté : {points}, Score total : {score}");
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + score;
            //Debug.Log($"UI mise à jour : Score : {score}");
        }
        else
        {
            Debug.LogError("scoreText n'est pas assigné dans le GameManager !");
        }
    }

    public void RestartGame()
    {
        Debug.Log("Redémarrage du jeu...");

        // Reprendre le temps s'il est en pause
        Time.timeScale = 1;

        // Sauvegarder le meilleur score avant de réinitialiser
        SaveBestScore();

        // Supprimer tous les objets inutiles dans la scène
        CleanUpScene();

        // Réinitialiser les Singletons (si nécessaire)
        ResetSingletons();

        // Recréer les objets essentiels comme la caméra, la lumière, etc.
        CreateEssentialObjects();

        // Réinitialiser l'état du jeu
        InitializeGame();
    }


    private void ResetSingletons()
    {
        // Réinitialisez vos Singletons manuellement si nécessaire
        if (countSoldat.Instance != null)
        {
            Destroy(countSoldat.Instance.gameObject);
        }
    }

    private void EnsureEssentialObjectsExist()
    {
        Debug.Log("Vérification des objets essentiels existants...");

        // Caméra
        if (Camera.main == null && mainCameraPrefab != null)
        {
            Instantiate(mainCameraPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("Main Camera créée.");
        }

        // Lumière directionnelle
        if (FindObjectOfType<Light>() == null && directionalLightPrefab != null)
        {
            Instantiate(directionalLightPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("Directional Light créée.");
        }

        // UI
        if (FindObjectOfType<Canvas>() == null && uiPrefab != null)
        {
            Instantiate(uiPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("UI créée.");
        }

        // EventSystem
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null && eventSystemPrefab != null)
        {
            Instantiate(eventSystemPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("EventSystem créé.");
        }

        // LineManager
        if (FindObjectOfType<LineGenerator>() == null && lineManagerPrefab != null)
        {
            currentLineManager = Instantiate(lineManagerPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("LineManager créé.");
        }
    }

    private void CreateEssentialObjects()
    {
        Debug.Log("Création des objets essentiels...");

        // LineManager
        if (lineManagerPrefab != null)
        {
            currentLineManager = Instantiate(lineManagerPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("LineManager créé.");
        }
    }

    private void CleanUpScene()
    {
        Debug.Log("Nettoyage de la scène...");
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.CompareTag("Clonable") || obj.CompareTag("Soldat") || obj.CompareTag("Portal")) // Supprimer les objets inutiles
            {
                Destroy(obj);
            }
        }
        Debug.Log("Nettoyage terminé.");
    }

    public void GameOver()
    {
        if (isGameOver) return; // Éviter d'appeler plusieurs fois GameOver

        Debug.Log("Game Over! Tous les soldats ont été éliminés.");
        isGameOver = true;

        // Arrêter la routine de score
        if (scoreCoroutine != null)
            StopCoroutine(scoreCoroutine);

        // Sauvegarder le meilleur score
        SaveBestScore();

        // Afficher l'écran de fin de partie
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Mettre à jour le meilleur score dans l'UI Game Over
        if (bestScoreText != null)
        {
            bestScoreText.text = $"Meilleur Score : {bestScore}";
        }

        // Arrêter le jeu (si nécessaire)
        Time.timeScale = 0; // Mettre le jeu en pause
    }

    private void SaveBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
            Debug.Log($"Nouveau meilleur score sauvegardé : {bestScore}");
        }
    }

    private void LoadBestScore()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = PlayerPrefs.GetInt("BestScore");
            Debug.Log($"Meilleur score chargé : {bestScore}");
        }
    }
}