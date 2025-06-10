using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public static LevelSelectionManager Instance;

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
        }
        else
        {
            Destroy(gameObject);
        }
    }

   public void OnEnable()
    {
        UpdateAllButtons();
    }

    void LoadProgress()
    {
        highestUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
    }

    void SaveProgress()
    {
        PlayerPrefs.SetInt("UnlockedLevel", highestUnlockedLevel);
        PlayerPrefs.Save();
    }

    public void CompleteLevel(int completedLevel)
    {
        if (completedLevel >= highestUnlockedLevel)
        {
            highestUnlockedLevel = completedLevel + 1;
            SaveProgress();
        }
    }

    public void LoadLevel(int levelNumber)
    {
        if (levelNumber <= highestUnlockedLevel)
        {
            SceneManager.LoadScene("Level" + levelNumber);
        }
    }

    public void UpdateAllButtons()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;
            bool isUnlocked = levelIndex <= highestUnlockedLevel;

            levelButtons[i].interactable = isUnlocked;
            levelButtons[i].GetComponentInChildren<Image>().sprite = isUnlocked ? numberSprites[i] : lockSprite;

            // Clear and re-add listeners
            levelButtons[i].onClick.RemoveAllListeners();
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }
}