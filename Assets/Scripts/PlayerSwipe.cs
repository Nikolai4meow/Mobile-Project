using UnityEngine;

public class PlayerSwipe : MonoBehaviour
{
    public Player player;
    private Vector2 startPos;
    public int pixelDistance = 20;
    private bool isFingerDown;

    private void Update()
    {
        if (!isFingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) // detects if finger is down for the first time 
        {
            startPos = Input.touches[0].position; // startPos is where we put our finger down
            isFingerDown = true; //the finger is down on the screen
        }

        if (isFingerDown)
        {
            if (Input.touches[0].position.y >= startPos.y + pixelDistance)
            {
                isFingerDown = false;
                Debug.Log("Up");
                player.Move(Vector3.forward);
            }
            else if (Input.touches[0].position.y <= startPos.y - pixelDistance)
            {
                isFingerDown = false;
                Debug.Log("Down");
                player.Move(-Vector3.forward);
            }
            else if (Input.touches[0].position.x <= startPos.x - pixelDistance)
            {
                isFingerDown = false;
                Debug.Log("Left");
                player.Move(Vector3.left);
            }
            else if (Input.touches[0].position.x >= startPos.x + pixelDistance)
            {
                isFingerDown = false;
                Debug.Log("Right");
                player.Move(Vector3.right);
            }

        }
    }
}
