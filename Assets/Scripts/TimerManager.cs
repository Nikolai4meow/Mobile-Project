using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance; // singleton

    [Header("Settings")]
    public float levelTime = 20f; // Set this per level

    [Header("UI References")]
    public TMP_Text timerText;
    public GameObject gameOverPanel;
    public Button restartButton;

    private float currentTime;
    private bool timerActive;

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
        currentTime = levelTime;
        timerActive = true;
        gameOverPanel.SetActive(false);

        restartButton.onClick.AddListener(RestartLevel);
        UpdateTimerDisplay();
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

        if (currentTime <= 5f) // Flash when time is low
        {
            timerText.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 0.5f));
        }
    }
    private void GameOver()
    { 
        Time.timeScale = 0f; // Pause game
        gameOverPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Unpause game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}