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
        SaveManager.Instance.SaveProgress(1);
        SceneManager.LoadScene("LevelSelection");
    }

    public void OnLoadGame()
    {
        SaveManager.Instance.SetCurrentUser(usernameInput.text);
        SceneManager.LoadScene("LevelSelection");
    }
}