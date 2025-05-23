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

        // Create a new material instance to avoid changing the original asset
        zombieMaterial = new Material(meshRenderer.material);
        meshRenderer.material = zombieMaterial;
        originalColor = zombieMaterial.color;
    }

    public void TakeDamage()
    {
        if (meshRenderer == null || zombieMaterial == null) return;

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
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        // Flash to red
        zombieMaterial.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        // Return to original color
        zombieMaterial.color = originalColor;
        flashRoutine = null;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        // Clean up the material instance when destroyed
        if (zombieMaterial != null)
        {
            Destroy(zombieMaterial);
        }
    }
}