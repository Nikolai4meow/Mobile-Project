using UnityEngine;

public class PlayerSwipe : MonoBehaviour
{
    public Player player;
    private Vector2 startPos;
    public int pixelDistance = 50;
    private bool isFingerDown;

    private void Update()
    {
        // Process keyboard input every frame
        ProcessKeyboardInput();

        // Only process touch input if player isn't moving
        if (!player.IsMoving)
        {
            ProcessTouchInput();
        }
        else
        {
            isFingerDown = false;
        }
    }

    private void ProcessKeyboardInput()
    {
        // Only process keyboard input if player isn't moving
        if (!player.IsMoving)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                player.SetMoveDirection(Vector3.forward);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                player.SetMoveDirection(Vector3.back);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                player.SetMoveDirection(Vector3.left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                player.SetMoveDirection(Vector3.right);
            }
        }
    }

    private void ProcessTouchInput()
    {
        if (Input.touchCount > 0)
        {
            if (!isFingerDown && Input.touches[0].phase == TouchPhase.Began)
            {
                startPos = Input.touches[0].position;
                isFingerDown = true;
            }

            if (isFingerDown)
            {
                Vector2 currentPos = Input.touches[0].position;
                Vector2 swipeDelta = currentPos - startPos;

                if (swipeDelta.magnitude > pixelDistance)
                {
                    isFingerDown = false;
                    if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    {
                        player.SetMoveDirection(swipeDelta.x > 0 ? Vector3.right : Vector3.left);
                    }
                    else
                    {
                        player.SetMoveDirection(swipeDelta.y > 0 ? Vector3.forward : Vector3.back);
                    }
                }
            }
        }
    }
}

