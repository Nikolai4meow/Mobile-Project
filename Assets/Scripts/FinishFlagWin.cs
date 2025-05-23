using UnityEngine;

public class FinishFlagWin : MonoBehaviour
{
    private void OnColliderEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("YOU WON!");
        }
    }
}
