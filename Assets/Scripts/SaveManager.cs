using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;
using static DifficultySettings;


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public DifficultySettings difficultySettings;
    [SerializeField] private TMP_Text messageText;
    private List<SaveData> allSaves = new List<SaveData>();
    public string currentUser;
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
        if (!SetCurrentUser(username)) // Modified this line
        {
            return false;
        }

        // Initialize new user data
        var newUser = new SaveData
        {
            username = username,
            highestUnlockedLevel = 1,
            userScore = 0,
            lastLoginDate = "" // Add this line to initialize the date field
        };

        allSaves.Add(newUser);
        SaveUserData(newUser);

        ShowMessage($"New user '{username}' created!");
        Debug.Log($"New user '{username}' created");
        return true;
    }

    public bool CheckUserDoesNotExist(string username)
    {
        // First check in memory
        bool existsInMemory = allSaves.Exists(s => s.username == username);
        if (existsInMemory)
        {
            return false;
        }

        // Then check in files
        string filePath = Path.Combine(savePath, $"{username}.json");
        bool existsInFiles = File.Exists(filePath);

        // Return true if user DOES NOT exist in either location
        return !existsInFiles;
    }

    public bool UserExists(string username)
    {
        // Check both in memory and in saved files
        bool inMemory = allSaves.Exists(s => s.username == username);
        if (inMemory) return true;

        string filePath = Path.Combine(savePath, $"{username}.json");
        return File.Exists(filePath);
    }

    public void SaveUserData(SaveData data)
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
    public bool SetCurrentUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            Debug.LogError("Username cannot be null or empty!");
            return false;
        }

        currentUser = username;
        PlayerPrefs.SetString("CurrentUser", username);
        PlayerPrefs.Save();
        Debug.Log($"Current user set to: {username}");
        return true;
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




      //  DifficultySettings.Difficulty diff = DifficultySettings.Instance.currentDifficulty;

      //  switch (diff) // to add gamescore based on the difficulty completed
       // {
       //     case Difficulty.Easy:
       //         scoreAdded = 1;
       //         break;
        //    case Difficulty.Medium:
        //        scoreAdded = 2;
         //       break;
        //    case Difficulty.Hard:
         //       scoreAdded = 3;
         //       break;
       // }
       // Update Score
     //   userSave.userScore += scoreAdded;
        

        // Save to file
     //   string json = JsonUtility.ToJson(userSave);
      //  File.WriteAllText(savePath + currentUser + ".json", json);

       // OnScoresUpdated();
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
            Debug.Log("Created new saves directory");
            return;
        }

        // Clear existing data
        allSaves.Clear();

        // Load all save files with error handling
        foreach (string file in Directory.GetFiles(savePath, "*.json"))
        {
            try
            {
                string json = File.ReadAllText(file);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                if (data != null)
                {
                    allSaves.Add(data);
                    Debug.Log($"Loaded user: {data.username}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load save file {file}: {e.Message}");
            }
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
        LoadAllUsers();
        if (string.IsNullOrEmpty(currentUser))
        {
            Debug.LogWarning("No current user set when trying to get user data");
            return null;
        }

        var userData = allSaves.Find(s => s.username == currentUser);
        if (userData == null)
        {
            Debug.LogError($"Current user '{currentUser}' not found in loaded saves");
        }
        return userData;
    }
    


}