using UnityEngine;

public class TextureWrap : MonoBehaviour
{
    private MeshFilter textureMesh;
    private Mesh mesh;
    void Start()
    {
        textureMesh = GetComponent<MeshFilter>();
        mesh = textureMesh.mesh;
        Vector2[] uvMap = mesh.uv;
        
        // Front
        uvMap[0] = new Vector2(0.334f, 0);
        uvMap[1] = new Vector2(0.666f, 0);
        uvMap[2] = new Vector2(0.334f, 0.5f);
        uvMap[3] = new Vector2(0.666f, 0.5f);

        // Top
        uvMap[4] = new Vector2(0.334f, 1);
        uvMap[5] = new Vector2(0.667f, 1);
        uvMap[8] = new Vector2(0.334f, 0.5f);
        uvMap[9] = new Vector2(0.666f, 0.5f);

        // Back
        uvMap[6] = new Vector2(1, 0.501f);
        uvMap[7] = new Vector2(0.666f, 0.5f);
        uvMap[10] = new Vector2(1, 1);
        uvMap[11] = new Vector2(0.666f, 1);

        // Bottom
        uvMap[12] = new Vector2(0.333f, 1);
        uvMap[13] = new Vector2(0.334f, 0.5f);
        uvMap[14] = new Vector2(0, 0.5f);
        uvMap[15] = new Vector2(0, 1);
        
        // Right
        uvMap[16] = new Vector2(0.667f, 0);
        uvMap[17] = new Vector2(0.667f, 0.501f);
        uvMap[18] = new Vector2(1, 0.5f);
        uvMap[19] = new Vector2(1, 0);
        
        // Left
        uvMap[20] = new Vector2(0, 0);
        uvMap[21] = new Vector2(0, 0.5f);
        uvMap[22] = new Vector2(0.333f, 0.5f);
        uvMap[23] = new Vector2(0.333f, 0);
        
        mesh.uv = uvMap;
    }
}
