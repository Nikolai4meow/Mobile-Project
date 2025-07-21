using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ScoreDisplay : MonoBehaviour

{
    [SerializeField] private TMP_Text scoreText;
    public static ScoreDisplay instance;

    private void OnEnable()
    {
        UpdateScoreDisplay();
        instance = this;
    }

    public void UpdateScoreDisplay()
    {
        if (scoreText == null)
        {
            Debug.LogError("Score Text reference not set!");
            return;
        }

        try
        {
            var currentUser = PlayerPrefs.GetString("CurrentUser");
            if (string.IsNullOrEmpty(currentUser))
            {
                scoreText.text = "No user selected";
                return;
            }

            var userData = SaveManager.Instance.GetCurrentUserData();
            if (userData != null)
            {
                scoreText.text = $"Score: {userData.userScore}";
                Debug.Log($"Displaying score: {userData.userScore} for {userData.username}");
            }
            else
            {
                scoreText.text = "Score: 0";
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error updating score: {ex.Message}");
            scoreText.text = "Score: Error";
        }
    }
   
}