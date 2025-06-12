using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelSelectionManager : MonoBehaviour
{
    public static LevelSelectionManager Instance { get; private set; }
    public static bool InstanceExists => Instance != null;

    [Header("UI References")]
    public Button[] levelButtons;
    public Sprite[] numberSprites;
    public Sprite lockSprite;

    private int highestUnlockedLevel = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
            Debug.Log($"Initial unlock: {highestUnlockedLevel}");
        }
        else
        {
            Debug.LogWarning("Duplicate LevelSelectionManager destroyed");
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LevelSelection")
        {
            // Re-cache buttons every time
            levelButtons = GameObject.FindGameObjectsWithTag("LevelButton")
                              .OrderBy(b => b.name)
                              .Select(b => b.GetComponent<Button>())
                              .ToArray();
            UpdateAllButtons();
        }
    }

    void LoadProgress()
    {
        highestUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
    }

    public void CompleteLevel(int completedLevel)
    {
      
       
            Debug.Log($"Completing level {completedLevel}. Current highest: {highestUnlockedLevel}");

            if (completedLevel >= highestUnlockedLevel - 1)
            {
                highestUnlockedLevel = completedLevel + 1;
                PlayerPrefs.SetInt("UnlockedLevel", highestUnlockedLevel);
                PlayerPrefs.Save();
                Debug.Log($"New highest unlocked: {highestUnlockedLevel}");
            }

        UpdateAllButtons();
    }

    void UpdateAllButtons()
    {
        
            if (levelButtons == null || levelButtons.Length == 0)
            {
                Debug.LogError("Level buttons not assigned!");
                return;
            }

            for (int i = 0; i < levelButtons.Length; i++)
            {
                int levelNumber = i + 1;
                bool isUnlocked = levelNumber <= highestUnlockedLevel;

                // Skip if button reference is null
                if (levelButtons[i] == null) continue;

                // Set button state
                levelButtons[i].interactable = isUnlocked;

                // Find the Image component that shows the number/lock
                Image buttonImage = levelButtons[i].GetComponentInChildren<Image>(true); // Include inactive
                if (buttonImage != null)
                {
                    buttonImage.sprite = isUnlocked ? numberSprites[i] : lockSprite;
                    buttonImage.gameObject.SetActive(true); // Ensure it's visible
                }

                // Remove and re-add click handler
                levelButtons[i].onClick.RemoveAllListeners();
                levelButtons[i].onClick.AddListener(() => LoadLevel(levelNumber));

                // Debug output
                Debug.Log($"Level {levelNumber} - Unlocked: {isUnlocked}");
            }

    }

    public void LoadLevel(int levelNumber)
    {
        Debug.Log($"Loading level {levelNumber}");
        SceneManager.LoadScene("Level" + levelNumber);
    }
}

   
