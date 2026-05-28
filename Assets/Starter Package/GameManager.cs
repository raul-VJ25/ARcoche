using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject timerText;
    public GameObject packagesCollectedText;
    public GameObject gameOverScreen;

    [Header("Game Settings")]
    public float gameTime = 120f; // 2 minutes in seconds

    private float currentTime;
    private int packagesCollected = 0;
    private bool gameEnded = false;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentTime = gameTime;
        packagesCollected = 0;
        gameEnded = false;

        // Show timer and counter, hide game over screen
        if (timerText != null)
            timerText.SetActive(true);
        if (packagesCollectedText != null)
            packagesCollectedText.SetActive(true);
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);

        UpdateUI();
    }

    private void Update()
    {
        if (gameEnded)
            return;

        // Countdown timer
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 0;
            EndGame();
        }

        UpdateUI();
    }

    public void AddPackage()
    {
        if (gameEnded)
            return;

        packagesCollected++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Update timer display (MM:SS format)
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.GetComponent<TextMeshProUGUI>().text =
                string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // Update packages counter
        if (packagesCollectedText != null)
        {
            packagesCollectedText.GetComponent<TextMeshProUGUI>().text =
                "Regalos: " + packagesCollected;
        }
    }

    private void EndGame()
    {
        gameEnded = true;

        // Show game over screen
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);

            // Update final score text if exists
            var finalScoreText = gameOverScreen.transform.Find("FinalScoreText")?.GetComponent<TextMeshProUGUI>();
            if (finalScoreText != null)
            {
                finalScoreText.text = "Regalos Recogidos: " + packagesCollected;
            }
        }

        // Hide timer and counter
        if (timerText != null)
            timerText.SetActive(false);
        if (packagesCollectedText != null)
            packagesCollectedText.SetActive(false);
    }

    public void RestartGame()
    {
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}