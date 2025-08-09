using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    public SceneMusic[] sceneMusicList;
    public AudioSource audioSource;
    private string currentSceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string newSceneName = scene.name;

        if (newSceneName != currentSceneName)
        {
            currentSceneName = newSceneName;
            PlaySceneMusic(newSceneName);
        }
    }

    private void PlaySceneMusic(string sceneName)
    {
        
        audioSource.Stop();

        foreach (var sceneMusic in sceneMusicList)
        {
            if (sceneMusic.sceneName == sceneName && sceneMusic.musicClip != null)
            {
                audioSource.clip = sceneMusic.musicClip;
                audioSource.Play();
                return;
            }
        }

        Debug.LogWarning($"No music found for scene: {sceneName}");
    }
}