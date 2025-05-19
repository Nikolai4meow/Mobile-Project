using UnityEngine;

public class Player : MonoBehaviour
{
   
    public float moveSpeed;
    private Vector3 moveDirection;
    private bool isMoving;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Requires Rigidbody component
        isMoving = false;
    }

    public void SetMoveDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            moveDirection = direction.normalized; // Ensures consistent speed
            isMoving = true;
        }
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.linearVelocity = moveDirection * moveSpeed; // Continuous movement
        }
        else
        {
            rb.linearVelocity = Vector3.zero; // Full stop
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Stops on wall collision
        {
            StopMoving();
        }
        else if (collision.gameObject.CompareTag("Zombie"))
        {
           StopMoving();

            ZombieController zombie = collision.gameObject.GetComponent<ZombieController>();
            if (zombie != null)
            {
                zombie.zombieHit();
            }
        }
    }
}


