using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public static LevelSelectionManager Instance;

    [Header("UI References")]
    public Button[] levelButtons;
    public Sprite[] numberSprites;
    public Sprite lockSprite;

    private int highestUnlockedLevel = 1;

    void Awake()
    {
        void Awake()
        {
            if (LevelSelectionManager.Instance == null)
            {
                Instantiate(Resources.Load<GameObject>("LevelSelectionManager"));
            }
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
            UpdateAllButtons();
        }
    }

    void LoadProgress()
    {
        highestUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        Debug.Log($"Loaded progress. Highest unlocked: {highestUnlockedLevel}");
    }

    public void CompleteLevel(int completedLevel)
    {
        if (completedLevel >= highestUnlockedLevel)
        {
            highestUnlockedLevel = completedLevel + 1;
            PlayerPrefs.SetInt("UnlockedLevel", highestUnlockedLevel);
            PlayerPrefs.Save();
            Debug.Log($"Unlocked up to level {highestUnlockedLevel}");
        }
    }

    public void UpdateAllButtons()
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

            if (levelButtons[i] == null) continue;

            // Update button appearance
            levelButtons[i].interactable = isUnlocked;

            Image buttonImage = levelButtons[i].GetComponentInChildren<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = isUnlocked ? numberSprites[i] : lockSprite;
            }

            // Clear and re-assign click handler
            levelButtons[i].onClick.RemoveAllListeners();
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelNumber));
        }
    }

    public void LoadLevel(int levelNumber)
    {
        if (levelNumber <= highestUnlockedLevel)
        {
            string sceneName = "Level" + levelNumber;
            Debug.Log($"Loading {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log($"Level {levelNumber} is locked!");
        }
    }
}