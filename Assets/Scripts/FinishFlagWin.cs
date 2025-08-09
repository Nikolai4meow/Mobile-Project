using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using static DifficultySettings;

public class FinishFlagWin : MonoBehaviour
{
    public DifficultySettings difficultySettings;
    private List<SaveData> allSaves = new List<SaveData>();
    private string currentUser;
    private string savePath;
   

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

       
        if (string.IsNullOrEmpty(SaveManager.Instance.currentUser))
        {
            Debug.LogError("Cannot save score - no current user set!");
            return;
        }
        


        var userSave = SaveManager.Instance.GetCurrentUserData();
        if (userSave == null)
        {
            Debug.LogError("Failed to load user data!");
            return;
        }
        int completedLevel = int.Parse(SceneManager.GetActiveScene().name.Replace("Level", ""));
        if (completedLevel >= userSave.highestUnlockedLevel)
        {
            userSave.highestUnlockedLevel = completedLevel + 1;
        }
        Debug.Log($"Level completed by: {SaveManager.Instance.currentUser}");

       
        int scoreToAdd = 0;
        switch (DifficultySettings.Instance.currentDifficulty)
        {
            case Difficulty.Easy:
                scoreToAdd = 1;
                break;
            case Difficulty.Medium:
                scoreToAdd = 2;
                break;
            case Difficulty.Hard:
                scoreToAdd = 3;
                break;
        }

        
        userSave.userScore += scoreToAdd;
        SaveManager.Instance.SaveUserData(userSave);
        SaveManager.Instance.OnScoresUpdated();

        Debug.Log($"Added {scoreToAdd} points! Total: {userSave.userScore}");

       
        LevelManager.Instance.ShowLevelComplete();
        AudioManager.Instance.PlaySound("Cheer");

    }
}

