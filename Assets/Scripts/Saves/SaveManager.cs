using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string currentUser;
    private string savePath;
    private Dictionary<string, SaveData> allSaves = new Dictionary<string, SaveData>();

    public string CurrentUser => currentUser; // Public getter
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

    // Call this when creating/selecting a user
    public bool SetCurrentUser(string username, bool isNewUser = false)
    {
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("Username cannot be empty!");
            return false;
        }

        if (isNewUser)
        {
            if (allSaves.ContainsKey(username))
            {
                Debug.Log($"User '{username}' already exists!");
                return false;
            }

            // NEW: Explicitly set starting progress to Level 1
            allSaves[username] = new SaveData
            {
                username = username,
                highestUnlockedLevel = 1 // Force Level 1 for new users
            };
            SaveUser(username);
            Debug.Log($"New user '{username}' created with starting level 1");
        }
        else
        {
            if (!allSaves.ContainsKey(username))
            {
                Debug.Log($"User '{username}' doesn't exist!");
                return false;
            }
        }

        currentUser = username;
        return true;
    }

    public bool LoadUser(string username)
    {
        if (allSaves.TryGetValue(username, out SaveData data))
        {
            currentUser = username;
            Debug.Log($"Loaded user: {username}");
            return true;
        }
        Debug.Log($"User '{username}' not found");
        return false;
    }

    public void SaveProgress(int levelReached)
    {
        if (string.IsNullOrEmpty(currentUser)) return;

        if (levelReached > allSaves[currentUser].highestUnlockedLevel)
        {
            allSaves[currentUser].highestUnlockedLevel = levelReached;
            SaveUser(currentUser);
        }
    }

    public int GetCurrentUserProgress()
    {
        return allSaves.TryGetValue(currentUser, out SaveData data)
            ? data.highestUnlockedLevel
            : 1;
    }

    private void SaveUser(string username)
    {
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        string json = JsonUtility.ToJson(allSaves[username]);
        File.WriteAllText($"{savePath}{username}.json", json);
    }

    private void LoadAllUsers()
    {
        if (!Directory.Exists(savePath)) return;

        foreach (string file in Directory.GetFiles(savePath, "*.json"))
        {
            string json = File.ReadAllText(file);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            allSaves[data.username] = data;
        }
    }

    public bool UserExists(string username)
    {
        return allSaves.ContainsKey(username);
    }
    public int LoadUserProgress()
    {
        if (string.IsNullOrEmpty(currentUser))
        {
            Debug.Log("No current user - defaulting to level 1");
            return 1;
        }
        return allSaves.TryGetValue(currentUser, out SaveData data)
            ? data.highestUnlockedLevel
            : 1;
    }
}