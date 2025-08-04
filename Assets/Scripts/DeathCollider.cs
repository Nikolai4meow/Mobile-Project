using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathCollider : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button restartButton;
    
    void Start()
    {
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(TimerManager.Instance.RestartLevel);
    }

  
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TimerManager.Instance.GameOver();
        }





    }
}
