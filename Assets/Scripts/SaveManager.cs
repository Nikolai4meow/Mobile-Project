using UnityEngine;
using System.IO;
using System.Collections.Generic;
using static DifficultySettings;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private List<SaveData> allSaves = new List<SaveData>();
    private string currentUser;
    private string savePath;

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
    public void SetCurrentUser(string username)
    {
        currentUser = username;
    }

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

        // Save to file
        string json = JsonUtility.ToJson(userSave);
        File.WriteAllText(savePath + currentUser + ".json", json);
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
    
   
}