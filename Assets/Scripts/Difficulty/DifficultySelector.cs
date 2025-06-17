using UnityEngine;

//DifficultySelector.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DifficultySelector : MonoBehaviour
{
    public DifficultySettings difficultySettings;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    private void Start()
    {
        // Highlight the current difficulty
        UpdateButtonStates();

        // Add button listeners
        easyButton.onClick.AddListener(() => SetDifficulty(DifficultySettings.Difficulty.Easy));
        mediumButton.onClick.AddListener(() => SetDifficulty(DifficultySettings.Difficulty.Medium));
        hardButton.onClick.AddListener(() => SetDifficulty(DifficultySettings.Difficulty.Hard));
    }

    private void SetDifficulty(DifficultySettings.Difficulty difficulty)
    {
        difficultySettings.currentDifficulty = difficulty;
        UpdateButtonStates();

        // Optional: Save the preference
        PlayerPrefs.SetInt("GameDifficulty", (int)difficulty);
    }

    private void UpdateButtonStates()
    {
        // Visual feedback for selected difficulty
        easyButton.interactable = difficultySettings.currentDifficulty != DifficultySettings.Difficulty.Easy;
        mediumButton.interactable = difficultySettings.currentDifficulty != DifficultySettings.Difficulty.Medium;
        hardButton.interactable = difficultySettings.currentDifficulty != DifficultySettings.Difficulty.Hard;
    }

    private void LoadDifficulty()
    {
        if (PlayerPrefs.HasKey("GameDifficulty"))
        {
            int savedDifficulty = PlayerPrefs.GetInt("GameDifficulty");
            difficultySettings.currentDifficulty = (DifficultySettings.Difficulty)savedDifficulty;
        }
    }

    private void OnEnable()
    {
        LoadDifficulty();
    }
}