using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelSelectionManager : MonoBehaviour
{
    public static LevelSelectionManager Instance;

    [Header("UI References")]
    public Button[] levelButtons;
    public Sprite[] numberSprites;
    public Sprite lockSprite;

    private int highestUnlockedLevel = 1; // Start with only level 1 unlocked

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
        }
        else
        {
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
            // Safely refresh button references
            levelButtons = GameObject.FindGameObjectsWithTag("LevelButton")
                  .OrderBy(go => {
                      string numStr = go.name.Replace("Level", "");
                      return int.Parse(numStr); // Proper numeric sorting
                  })
                  .Select(go => go.GetComponent<Button>())
                  .ToArray();

            UpdateAllButtons();
        }
    }

    void LoadProgress()
    {
        
        if (SaveManager.Instance != null)
        {
            highestUnlockedLevel = SaveManager.Instance.LoadUserProgress();
        }
        else
        {
            highestUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        }
    }

    [System.Obsolete]
    public void CompleteLevel(int completedLevel)
    {
        Debug.Log($"Completing level {completedLevel}. Current highest: {highestUnlockedLevel}");


        if (completedLevel >= highestUnlockedLevel - 1)
        {
            highestUnlockedLevel = completedLevel + 1;

            // Save to both systems
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.SaveProgress(highestUnlockedLevel);
            }
            PlayerPrefs.SetInt("UnlockedLevel", highestUnlockedLevel);
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

            if (levelButtons[i] == null)
            {
                Debug.LogError($"Button {i} is null!");
                continue;
            }

            // Set button state
            levelButtons[i].interactable = isUnlocked;

            // Find the Image component that shows the number/lock
            Image buttonImage = levelButtons[i].GetComponentInChildren<Image>(true);
            if (buttonImage != null)
            {
                buttonImage.sprite = isUnlocked ? numberSprites[i] : lockSprite;
                buttonImage.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError($"No Image found on button {levelNumber}");
            }

            // Update click handler
            levelButtons[i].onClick.RemoveAllListeners();
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelNumber));

            Debug.Log($"Level {i + 1} - Unlocked: {isUnlocked} - Sprite: {(isUnlocked ? numberSprites[i].name : lockSprite.name)}");
        }
    }

    public void LoadLevel(int levelNumber)
    {
        if (levelNumber <= highestUnlockedLevel)
        {
            SceneManager.LoadScene("Level" + levelNumber);
        }
    }

    
    public void DebugResetProgress()
    {
        PlayerPrefs.DeleteKey("UnlockedLevel");
        highestUnlockedLevel = 1;
        UpdateAllButtons();
        Debug.Log("Progress reset to level 1");
    }
}


