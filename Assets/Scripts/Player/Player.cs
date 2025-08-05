using UnityEngine;

public class Player : MonoBehaviour
{
   
    public float moveSpeed;
    private Vector3 moveDirection;
    private bool isMoving;
    private Rigidbody rb;

    public bool IsMoving { get { return isMoving; } }

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // requires Rigidbody component
        isMoving = false;
    }

    public void SetMoveDirection(Vector3 direction)
    {
        if (!isMoving) // move only when the player is not moving
        {
            moveDirection = direction.normalized; // gives move speed
            isMoving = true;
        }
    }
    

    public void StopMoving()
    {
        isMoving = false;
        AudioManager.Instance.PlaySound("Crubling");
        AnimationManager.Instance.PlayAnimation("Girl Idle", "Girl");
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.linearVelocity = moveDirection * moveSpeed; // continuous movement
        }
        else
        {
            rb.linearVelocity = Vector3.zero; // stop
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // stops on wall collision
        {
            StopMoving();
        }
        else if (collision.gameObject.CompareTag("Zombie"))
        {
            StopMoving();

            ZombieController zombie = collision.gameObject.GetComponent<ZombieController>();
            if (zombie != null)
            {
                zombie.TakeDamage();

            }
        }
    }
}