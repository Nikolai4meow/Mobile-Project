
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextButton;

    private int currentLevelNumber; // Track which level this is

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Extract level number from scene name
        string sceneName = SceneManager.GetActiveScene().name;
        if (int.TryParse(sceneName.Replace("Level", ""), out currentLevelNumber))
        {
            Debug.Log($"Current level: {currentLevelNumber}");
        }
    }

    private void Start()
    {
        levelCompletePanel.SetActive(false);
        restartButton.onClick.AddListener(RestartLevel);
        nextButton.onClick.AddListener(NextLevel);
    }

    public void ShowLevelComplete()
    {
        levelCompletePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [System.Obsolete]
    private void NextLevel()
    {

        Time.timeScale = 1f;

        // Get level number from current scene name (e.g., "Level3" → 3)
        int completedLevel = int.Parse(SceneManager.GetActiveScene().name.Replace("Level", ""));

        LevelSelectionManager.Instance.CompleteLevel(completedLevel); // Pass ACTUAL level
        SceneManager.LoadScene("LevelSelection");

    }
}