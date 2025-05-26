using UnityEngine;

public class FinishFlagWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("YOU WON!");
            LevelManager.Instance.ShowLevelComplete();
        }
    }
}
