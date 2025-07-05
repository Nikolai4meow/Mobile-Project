using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyRewardUI : MonoBehaviour
{
    public static DailyRewardUI Instance;

    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text streakText;
    [SerializeField] private Image rewardImage;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowReward(string message, int points)
    {
        rewardText.text = $"You earned {points} points!";
        streakText.text = message;
        rewardPanel.SetActive(true);

        // Auto-hide after 3 seconds
        Invoke("HideReward", 3f);
    }

    private void HideReward()
    {
        rewardPanel.SetActive(false);
    }
}
