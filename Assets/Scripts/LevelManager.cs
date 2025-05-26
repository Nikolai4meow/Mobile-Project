
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance; // This is a singleton

    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextButton;

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
        // Hide panel at start
        levelCompletePanel.SetActive(false);

        // Setup button listeners
        restartButton.onClick.AddListener(RestartLevel);
        nextButton.onClick.AddListener(NextLevel);
    }

    public void ShowLevelComplete()
    {
        levelCompletePanel.SetActive(true);
        // Pause game if needed
        Time.timeScale = 0f;
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void NextLevel()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

       
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}