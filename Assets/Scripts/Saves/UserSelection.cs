using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserSelection : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public Button newUserButton;
    public Button loadUserButton;

    void Start()
    {
        // Button listeners
        newUserButton.onClick.AddListener(OnNewUser);
        loadUserButton.onClick.AddListener(OnLoadUser);
    }

    private void OnNewUser()
    {
        string username = usernameInput.text.Trim();
        if (SaveManager.Instance.SetCurrentUser(username, true))
        {
            // NEW: Immediate verification
            int startingLevel = SaveManager.Instance.GetCurrentUserProgress();
            Debug.Log($"New user '{username}' created. Verify starting level: {startingLevel}");

            SceneManager.LoadScene("MainMenu Screen");
        }
    }

    private void OnLoadUser()
    {
        string username = usernameInput.text.Trim();
        if (!SaveManager.Instance.UserExists(username))
        {
            Debug.Log($"Cannot load - user '{username}' doesn't exist!");
            return;
        }

        if (SaveManager.Instance.LoadUser(username))
        {
            SceneHistoryManager.Instance.RecordCurrentScene();
            SceneManager.LoadScene("MainMenu Screen");
        }
    }
}