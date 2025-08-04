using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    [Header("Settings")]
    public float baseLevelTime = 10f; // Base time for medium difficulty
    public DifficultySettings difficultySettings;

    [Header("UI References")]
    public TMP_Text timerText;
    public GameObject gameOverPanel;
    public Button restartButton;

    private float currentTime;
    private bool timerActive;
    private float levelTime; // Actual time after difficulty adjustments

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ApplyDifficultySettings();
        currentTime = levelTime;
        timerActive = true;
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartLevel);
        UpdateTimerDisplay();
    }

    private void ApplyDifficultySettings()
    {
        if (difficultySettings == null) return;

        switch (difficultySettings.currentDifficulty)
        {
            case DifficultySettings.Difficulty.Easy:
                levelTime = baseLevelTime + difficultySettings.easyTimeBonus;
                break;
            case DifficultySettings.Difficulty.Medium:
                levelTime = baseLevelTime;
                break;
            case DifficultySettings.Difficulty.Hard:
                levelTime = baseLevelTime - difficultySettings.hardTimePenalty;
                break;
        }
    }


    private void Update()
    {
        if (timerActive)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (currentTime <= 0)
            {
                currentTime = 0;
                timerActive = false;
                GameOver();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        timerText.text = Mathf.CeilToInt(currentTime).ToString();

        if (currentTime <= 5f)
        {
            timerText.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 0.5f));
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}