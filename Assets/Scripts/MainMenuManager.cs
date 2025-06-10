using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnStartPressed()
    {
        // Load your game scene - replace "GameScene" with your actual scene name
        SceneManager.LoadScene("LevelSelection");
    }

    public void OnExitPressed()
    {
        // Quit the application
        Application.Quit();

        // For testing in editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
