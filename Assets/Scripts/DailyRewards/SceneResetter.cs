using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for Button interaction

public class SceneResetter : MonoBehaviour
{
    [SerializeField] private float resetDelay = 0.5f;
    [SerializeField] private Button resetButton; // Assign in Inspector

    void Start()
    {
        // Automatically hook up the button
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetScene);
        }
    }

    public void ResetScene()
    {
        StartCoroutine(ResetSceneWithDelay(resetDelay));
    }

    IEnumerator ResetSceneWithDelay(float delay)
    {
        // Optional: Add visual effects here
        Debug.Log("Scene reset initiated");

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}