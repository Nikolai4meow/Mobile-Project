using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UserSelection : MonoBehaviour
{
    public TMP_InputField usernameInput;

    public void OnNewGame()
    {
        SaveManager.Instance.SetCurrentUser(usernameInput.text);
        string newUsername = usernameInput.text.Trim();

        if (!SaveManager.Instance.TryCreateNewUser(newUsername))
        {
            // The error message is already handled by SaveManager
            return;
        }
        SaveManager.Instance.SaveProgress(1);
        SceneManager.LoadScene("LevelSelection");
    }

    public void OnLoadGame()
    {
        SaveManager.Instance.SetCurrentUser(usernameInput.text);
        SceneManager.LoadScene("LevelSelection");
    }
}