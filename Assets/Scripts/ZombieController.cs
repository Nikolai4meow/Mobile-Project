using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public ZombieData zombieData;
    private int currentHits;

    void awake()
    {
        
        createZombie();
    }

    void createZombie()
    {
        currentHits = 0;
        
    }

    public void zombieHit()
    {
        currentHits++;
        if (currentHits >= zombieData.hitsToKill)
        {
            ZombieDie();
        }
       

        
    }

    private void ZombieDie()
    {
        Destroy(gameObject);
    }
}
