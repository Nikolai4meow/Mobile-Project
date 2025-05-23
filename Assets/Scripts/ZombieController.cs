using System.Collections;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public ZombieData zombieData;
    private int currentHits;
    private MeshRenderer meshRenderer;
    private Color originalColor;
    private Coroutine flashRoutine;



    void awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
        createZombie();
    }

    void createZombie()
    {
        currentHits = 0;
        meshRenderer.material.color = originalColor;

    }

    public void zombieHit()
    {
        currentHits++;
        
        if (currentHits < zombieData.hitsToKill)
        {
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }
            flashRoutine = StartCoroutine(FlashRed());
        }
        else
        {
            ZombieDie();

        }
        IEnumerator FlashRed()
        {
            // Flash to red
            meshRenderer.material.color = Color.red;

            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Return to default color
            meshRenderer.material.color = originalColor;

            flashRoutine = null;
        }

        void ZombieDie()
        {
            Destroy(gameObject);
        }
    }
}
