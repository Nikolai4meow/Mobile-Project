using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;
using static DifficultySettings;
using System;


public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameObject dailyRewardUIPrefab;
    private DailyRewardUI rewardUIInstance;

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
        if (DailyRewardUI.Instance == null && dailyRewardUIPrefab != null)
        {
            var ui = Instantiate(dailyRewardUIPrefab);
            rewardUIInstance = ui.GetComponent<DailyRewardUI>();
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
    public void CheckDailyReward(string username)
    {
        var userData = allSaves.Find(s => s.username == username);
        if (userData == null) return;

        DateTime today = DateTime.Today;
        DateTime lastLogin = string.IsNullOrEmpty(userData.lastLoginDate)
            ? today.AddDays(-1) // Force reward on first login
            : DateTime.Parse(userData.lastLoginDate);

        if (lastLogin < today)
        {
            // Update the date FIRST
            userData.lastLoginDate = today.ToString("yyyy-MM-dd");

            // Calculate reward
            int rewardPoints = 1 + (userData.consecutiveLoginDays / 3);
            userData.userScore += rewardPoints;

            // FORCE SAVE (critical step)
            SaveUserData(userData); // This was likely missing before

            Debug.Log($"Saved lastLoginDate: {userData.lastLoginDate}");
        }
    }

    private int CalculateDailyReward(int streak)
    {
        // Base reward + bonus for streak
        int baseReward = 1;
        int streakBonus = Mathf.FloorToInt(streak/ 3); // Extra point every 3 days

        return baseReward + streakBonus;
    }

    private void ShowDailyRewardMessage(int points, int streak)
    {
        string message = $"Daily Reward: +{points} points!\n";
        message += $"Login Streak: {streak} days";

        if (DailyRewardUI.Instance != null)
        {
            DailyRewardUI.Instance.ShowReward(message, points);
        }
        else if (rewardUIInstance != null)
        {
            rewardUIInstance.ShowReward(message, points);
        }
        Debug.Log(message);
    }

    // Call this when loading a user
    public void LoadUser(string username)
    {
        SetCurrentUser(username);
        CheckDailyReward(username);
        OnScoresUpdated(); // Refresh displays
    }
    private bool CheckForTimeCheat(DateTime lastLogin)
    {
        // If the last login appears to be in the future
        if (lastLogin > DateTime.Today)
        {
            Debug.LogWarning("Potential time cheating detected!");
            // Apply penalty or reset streak
            return true;
        }
        return false;
    }

    [ContextMenu("Print Save Path")]
    public void PrintSavePath()
    {
        Debug.Log($"Save files are located at:\n{savePath}");

        if (Directory.Exists(savePath))
        {
            Debug.Log("Found these save files:");
            foreach (string file in Directory.GetFiles(savePath, "*.json"))
            {
                Debug.Log(file);
                Debug.Log(File.ReadAllText(file));
            }
        }
        else
        {
            Debug.LogWarning("Save directory doesn't exist yet!");
        }
    }



}