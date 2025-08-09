using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour
{
    public ZombieData zombieData;
    private int currentHits;
    private MeshRenderer meshRenderer;
    private Color originalColor;
    private Coroutine flashRoutine;
    private Material zombieMaterial;

    void Awake()
    {
        // Safely get the MeshRenderer
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer not found on zombie!", this);
            return;
        }

        zombieMaterial = new Material(meshRenderer.material);
        meshRenderer.material = zombieMaterial;
        originalColor = zombieMaterial.color;
    }

    public void TakeDamage()
    {
        if (meshRenderer == null || zombieMaterial == null) return;

        currentHits++;
        AudioManager.Instance.PlaySound("Swing");
       

        if (currentHits < zombieData.hitsToKill)
        {
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }
            flashRoutine = StartCoroutine(FlashRed());
            AudioManager.Instance.PlaySound("Zombie Hit");
        }
        else
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        
        zombieMaterial.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        
        zombieMaterial.color = originalColor;
        flashRoutine = null;
    }

    private void Die()
    {
        Destroy(gameObject);
        AudioManager.Instance.PlaySound("Zombie Die");
    }

    void OnDestroy()
    {
        
        if (zombieMaterial != null)
        {
            Destroy(zombieMaterial);
        }
    }
}