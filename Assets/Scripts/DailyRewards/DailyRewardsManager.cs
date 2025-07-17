using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SocialPlatforms;

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
        currentUser = username;
    }

    public void OnGiftButtonClicked()
    {
        //if (string.IsNullOrEmpty(currentUser))
      //  {
      //      Debug.LogError("No current user set!");
       //     return;
       // }

        string filePath = Path.Combine(savePath, $"{currentUser}.json");
        SaveData saveData;

        // Load or create new save
        if (File.Exists(filePath))
        {
            saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(filePath));
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

        DateTime today = DateTime.Today;
        DateTime lastLogin = string.IsNullOrEmpty(saveData.lastLoginDate)
            ? today.AddDays(-1) // Force reward panel on first time
            : DateTime.Parse(saveData.lastLoginDate);

        // Show appropriate panel
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
            SaveManager.Instance.OnScoresUpdated();

            File.WriteAllText(filePath, JsonUtility.ToJson(saveData));

            // Close ALL panels reliably
            DarkBackgroundPanel.SetActive(false);
            Rewards_Panel.SetActive(false);
            NoRewards_Panel.SetActive(false);

            Debug.Log($"Added 10 points. New score: {saveData.userScore}");
        }
        finally
        {
            isProcessingClaim = false;
        }

        DarkBackgroundPanel.SetActive(false);
        Rewards_Panel.SetActive(false);
        NoRewards_Panel.SetActive(false);

        StartCoroutine(EnableButtonAfterDelay(1f));

    }

    IEnumerator EnableButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rewardClaimButton.interactable = true;
    }
    
}
