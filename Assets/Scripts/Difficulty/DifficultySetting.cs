// DifficultySettings.cs
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySettings", menuName = "Game/Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    public enum Difficulty { Easy, Medium, Hard }

    [Header("Time Adjustments")]
    public float easyTimeBonus = 5f;
    public float hardTimePenalty = 3f;

    [Header("Score Adjustments")]
    public int easyScore = 1;
    public int mediumScore = 2;
    public int hardScore = 3;
    

    [Header("Current Difficulty")]
    public Difficulty currentDifficulty = Difficulty.Medium;

    private static DifficultySettings _instance;
    public static DifficultySettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<DifficultySettings>("DifficultySettings");
                if (_instance == null)
                    Debug.LogError("Create DifficultySettings asset in Resources folder!");
            }
            return _instance;
        }
    }

    // Helper property to get current score value
    public int CurrentScoreValue
    {
        get
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy: return easyScore;
                case Difficulty.Medium: return mediumScore;
                case Difficulty.Hard: return hardScore;
                default: return mediumScore;
            }
        }
    }
}
