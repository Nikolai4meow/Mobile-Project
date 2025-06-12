// DifficultySettings.cs
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySettings", menuName = "Game/Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    public enum Difficulty { Easy, Medium, Hard }

    [Header("Time Adjustments")]
    public float easyTimeBonus = 5f;
    public float hardTimePenalty = 3f;

    [Header("Current Difficulty")]
    public Difficulty currentDifficulty = Difficulty.Medium;
}