using UnityEngine;

public class SpriteCam : MonoBehaviour
{
    [SerializeField] private bool freezeYZAxis = true;
    private void Update()
    {
        if (freezeYZAxis)
        {
            transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, 0f, 0f);
        }
        else
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
