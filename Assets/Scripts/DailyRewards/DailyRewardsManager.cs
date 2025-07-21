using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;

public class DailyRewardManager : MonoBehaviour
{

    // UI References (assign in Inspector)
    public GameObject DarkBackgroundPanel;
    public GameObject Rewards_Panel;
    public GameObject NoRewards_Panel;
    public Button rewardClaimButton; // The GREEN claim button
    public Button giftButton; // Your gift icon button in main UI
    private SaveData saveData1;


    private bool isProcessingClaim = false;
    private string currentUser;
    private string savePath;

    private void Awake()
    {
        saveData1 = SaveManager.Instance.GetCurrentUserData();
        currentUser = saveData1.username;
        
            if (string.IsNullOrEmpty(savePath))
            {
                savePath = Path.Combine(Application.persistentDataPath, "saves");
            }
    }
    void Start()
    {

        savePath = Path.Combine(Application.persistentDataPath, "saves");
        Directory.CreateDirectory(savePath);
        Debug.Log($"from the dailyrewardsmanager current user is {currentUser}");

        // Setup button listeners
        giftButton.onClick.AddListener(OnGiftButtonClicked);

    }

    public void SetCurrentUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new System.ArgumentNullException("username cannot be null or empty");
        }
        currentUser = username;
    }

    public void OnGiftButtonClicked()
    {
        if (string.IsNullOrEmpty(currentUser))
        {
            // Try to load from PlayerPrefs as fallback
            currentUser = PlayerPrefs.GetString("CurrentUser");

            if (string.IsNullOrEmpty(currentUser))
            {
                Debug.LogError("No current user set! Please create or select a user first.");
                return;
            }
        }

        // 2. Verify save directory exists
        try
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to access save directory: {e.Message}");
            return;
        }

        // 3. Build file path safely
        string filePath;
        try
        {
            filePath = Path.Combine(savePath, $"{currentUser}.json");
            if (string.IsNullOrEmpty(filePath))
            {
                throw new System.Exception("Generated file path is invalid");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to create file path: {e.Message}");
            return;
        }

        // 4. Load or create save data
        SaveData saveData;
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                saveData = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                saveData = new SaveData
                {
                    username = currentUser,
                    userScore = 0,
                    highestUnlockedLevel = 1,
                    lastLoginDate = ""
                };
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load/create save data: {e.Message}");
            return;
        }

        // 5. Date comparison
        DateTime today = DateTime.Today;
        DateTime lastLogin;

        try
        {
            lastLogin = string.IsNullOrEmpty(saveData.lastLoginDate)
                ? today.AddDays(-1)
                : DateTime.Parse(saveData.lastLoginDate);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to parse login date: {e.Message}");
            lastLogin = today.AddDays(-1); // Default to force reward
        }

        // 6. Show appropriate panel
        try
        {
            DarkBackgroundPanel.SetActive(true);
            if (lastLogin.Date < today.Date)
            {
                Rewards_Panel.SetActive(true);
                NoRewards_Panel.SetActive(false);
            }
            else
            {
                NoRewards_Panel.SetActive(true);
                Rewards_Panel.SetActive(false);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to update UI: {e.Message}");
        }
    }

    [Obsolete]
    private void OnRewardClaimed()
    {
        Debug.Log($"Claim initiated at {Time.time}");

        if (isProcessingClaim) return;
        isProcessingClaim = true;

        rewardClaimButton.interactable = false;

        try
        {
            string filePath = Path.Combine(savePath, $"{currentUser}.json");
            SaveData saveData = File.Exists(filePath)
                ? JsonUtility.FromJson<SaveData>(File.ReadAllText(filePath))
                : new SaveData { username = currentUser };

            // Add EXACTLY 10 points
            saveData.userScore += 10;
            saveData.lastLoginDate = DateTime.Today.ToString("yyyy-MM-dd");
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(savePath + currentUser + ".json", json);
            OnScoresUpdated();

            File.WriteAllText(filePath, JsonUtility.ToJson(saveData));

            // Close ALL panels reliably
            DarkBackgroundPanel.SetActive(false);
            Rewards_Panel.SetActive(false);
            NoRewards_Panel.SetActive(false);

            Debug.Log($"Added 10 points. New score: {saveData.userScore}");
           OnScoresUpdated();
        }
        finally
        {
            isProcessingClaim = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            OnScoresUpdated();
        }

        DarkBackgroundPanel.SetActive(false);
        Rewards_Panel.SetActive(false);
        NoRewards_Panel.SetActive(false);

        StartCoroutine(EnableButtonAfterDelay(1f));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        OnScoresUpdated();
        

    }

    IEnumerator ResetSceneWithDelay(float delay = 0.5f)
    {
        
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    IEnumerator EnableButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rewardClaimButton.interactable = true;
    }


    [System.Obsolete]
    public void OnScoresUpdated()
    {
       ScoreDisplay.instance.UpdateScoreDisplay();
        // Call this whenever scores change
        ScoreDisplay[] displays = FindObjectsOfType<ScoreDisplay>();
        foreach (var display in displays)
        {
            display.UpdateScoreDisplay();
        }


    }
}
