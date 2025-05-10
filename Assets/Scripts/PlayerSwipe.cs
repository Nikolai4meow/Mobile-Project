using UnityEngine;

public class PlayerSwipe : MonoBehaviour
{

    public Player player;
    private Vector2 startPos;
    public int pixelDistance = 50;
    private bool isFingerDown;

    private void Update()
    {
        if (!isFingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            startPos = Input.touches[0].position;
            isFingerDown = true;
        }

        if (isFingerDown && Input.touchCount > 0)
        {
            Vector2 currentPos = Input.touches[0].position;
            Vector2 swipeDelta = currentPos - startPos;

            if (swipeDelta.magnitude > pixelDistance)
            {
                isFingerDown = false;

                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    // Horizontal swipe
                    player.SetMoveDirection(swipeDelta.x > 0 ? Vector3.right : Vector3.left);
                }
                else
                {
                    // Vertical swipe
                    player.SetMoveDirection(swipeDelta.y > 0 ? Vector3.forward : Vector3.back);
                }
            }
        }
    }
}

