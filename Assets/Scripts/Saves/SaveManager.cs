using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;
using static DifficultySettings;
using System;


public class SaveManager : MonoBehaviour
{
    
    

    public static SaveManager Instance;
    public DifficultySettings difficultySettings;
    [SerializeField] private TMP_Text messageText;
    private List<SaveData> allSaves = new List<SaveData>();
    private string currentUser;
    private string savePath;
   private int scoreAdded;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/saves/";
            LoadAllUsers();
        }
        else
        {
            Destroy(gameObject);
        }
       
    }
    public bool TryCreateNewUser(string username)
    {
        // Check if username is empty
        if (string.IsNullOrWhiteSpace(username))
        {
            ShowMessage("Username cannot be empty!");
            return false;
        }

        // Check if user already exists
        if (UserExists(username))
        {
            ShowMessage("User already exists!");
            Debug.Log($"User '{username}' already exists");
            return false;
        }

        // Create new user
        SetCurrentUser(username);

        // Initialize new user data
        var newUser = new SaveData
        {
            username = username,
            highestUnlockedLevel = 1,
            userScore = 0
        };

        allSaves.Add(newUser);
        SaveUserData(newUser);

        ShowMessage($"New user '{username}' created!");
        Debug.Log($"New user '{username}' created");
        return true;
    }

    private bool UserExists(string username)
    {
        // Check both in memory and in saved files
        bool inMemory = allSaves.Exists(s => s.username == username);
        if (inMemory) return true;

        string filePath = Path.Combine(savePath, $"{username}.json");
        return File.Exists(filePath);
    }

    private void SaveUserData(SaveData data)
    {
        Directory.CreateDirectory(savePath);
        string filePath = Path.Combine(savePath, $"{data.username}.json");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    private void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
           
        }
        Debug.Log(message);
    }

    // Call this when creating/selecting a user
    public void SetCurrentUser(string username)
    {
        if (!UserExists(username))
        {
            Debug.LogError($"Tried to set non-existent user: {username}");
            ShowMessage("User not found!");
            return;
        }

        currentUser = username;
        PlayerPrefs.SetString("CurrentUser", username);
        PlayerPrefs.Save();
        Debug.Log($"Current user set to: {username}");
    }

    [System.Obsolete]
    public void SaveProgress(int levelReached)
    {
        // Find or create save for current user
        var userSave = allSaves.Find(s => s.username == currentUser);
        if (userSave == null)
        {
            userSave = new SaveData { username = currentUser };
            allSaves.Add(userSave);
        }

        // Update progress
        if (levelReached > userSave.highestUnlockedLevel)
        {
            userSave.highestUnlockedLevel = levelReached;
        }





        DifficultySettings.Difficulty diff = DifficultySettings.Instance.currentDifficulty;
       
        switch (diff) // to add gamescore based on the difficulty completed
        {
            case Difficulty.Easy:
                scoreAdded = 1;
                break;
            case Difficulty.Medium:
                scoreAdded = 2;
                break;
            case Difficulty.Hard:
                scoreAdded = 3;
                break;
        }
                //Update Score
                userSave.userScore += scoreAdded;
        

        // Save to file
        string json = JsonUtility.ToJson(userSave);
        File.WriteAllText(savePath + currentUser + ".json", json);

        OnScoresUpdated();
    }

    public int LoadUserProgress()
    {
        var userSave = allSaves.Find(s => s.username == currentUser);
        return userSave?.highestUnlockedLevel ?? 1;
    }

    private void LoadAllUsers()
    {
        // Create saves directory if needed
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
            return;
        }

        // Load all save files
        foreach (string file in Directory.GetFiles(savePath, "*.json"))
        {
            string json = File.ReadAllText(file);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            allSaves.Add(data);
        }
    }

    [System.Obsolete]
    public void OnScoresUpdated()
    {
        // Call this whenever scores change
        ScoreDisplay[] displays = FindObjectsOfType<ScoreDisplay>();
        foreach (var display in displays)
        {
            display.UpdateScoreDisplay();
        }
    }
    public SaveData GetCurrentUserData()
    {
        return allSaves.Find(s => s.username == currentUser);
    }
   

}



 
   
   




