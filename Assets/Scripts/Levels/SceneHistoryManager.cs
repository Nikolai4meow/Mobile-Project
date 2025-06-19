using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneHistoryManager : MonoBehaviour
{
    public static SceneHistoryManager Instance;
    public GameObject backButtonPrefab;
    public List<string> ignoredScenes = new List<string> { "UserSelection" };

    private Stack<string> sceneHistory = new Stack<string>();

    public void ClearHistory() => sceneHistory.Clear();

    [System.Obsolete]
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Obsolete]
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Loaded: {scene.name}. History count: {sceneHistory.Count}");

        // Don't show back button in ignored scenes
        if (ignoredScenes.Contains(scene.name)) return;

        // Show back button if there's history
        if (sceneHistory.Count > 0 && backButtonPrefab != null)
        {
            CreateBackButton();
        }
    }

    public void RecordCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (sceneHistory.Count == 0 || sceneHistory.Peek() != currentScene)
        {
            sceneHistory.Push(currentScene);
            Debug.Log($"Recorded: {currentScene}. History: {string.Join("→", sceneHistory)}");
        }
    }

    public void GoBack()
    {
        if (sceneHistory.Count > 0)
        {
            string targetScene = sceneHistory.Pop();
            Debug.Log($"Going back from {SceneManager.GetActiveScene().name} to {targetScene}");
            SceneManager.LoadScene(targetScene);
        }
        else
        {
            Debug.Log("No history - loading fallback");
            SceneManager.LoadScene("UserSelection");
        }
    }

    [System.Obsolete]
    void CreateBackButton()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            Instantiate(backButtonPrefab, canvas.transform)
                .GetComponent<Button>().onClick.AddListener(GoBack);
        }
    }

    [System.Obsolete]
    public void TryShowBackButton()
    {
        if (sceneHistory.Count > 0 && !ignoredScenes.Contains(SceneManager.GetActiveScene().name))
        {
            CreateBackButton(); // Spawns your prefab
        }
    }
}