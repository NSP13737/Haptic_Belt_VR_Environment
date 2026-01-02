
using UnityEngine;
using UnityEngine.Rendering;

public class GrassInstancingTest : MonoBehaviour
{
    [Header("Assets")]
    public Mesh grassMesh;            // Should be a 2D quad or your grass mesh (submesh 0 must have triangles)
    public Material grassMaterial;    // Shader must support GPU Instancing and have it enabled

    [Header("Placement")]
    [SerializeField] [Range(0f,1023f)] private int instanceCount = 512; // <= 1023 per batch
    [SerializeField] private Vector2 areaExtents = new Vector2(10f, 10f); // XZ extents or XY depending on your plane
    [SerializeField] private float groundY = 0f; // If using XZ plane with Y as up

    [Header("Variation")]
    [SerializeField] private float minSize = 0.6f;  // Ensure > 0
    [SerializeField] private float maxSize = 1.4f;  // Ensure >= minSize
    [SerializeField] private float randomRot = 15f;

    private Matrix4x4[] matrices;

    void Awake()
    {
        // Clamp instance count to API limit
        instanceCount = Mathf.Clamp(instanceCount, 1, 1023);

        // Basic validation
        if (grassMesh == null)
        {
            Debug.LogError("GrassInstancingTest: grassMesh is not assigned.");
            enabled = false;
            return;
        }
        if (grassMaterial == null)
        {
            Debug.LogError("GrassInstancingTest: grassMaterial is not assigned.");
            enabled = false;
            return;
        }
        if (!grassMaterial.enableInstancing)
        {
            Debug.LogWarning("GrassInstancingTest: GPU Instancing is not enabled on grassMaterial. Enable it to render instances efficiently.");
        }
        if (minSize <= 0f)
        {
            Debug.LogWarning("GrassInstancingTest: minSize must be > 0. Setting to 0.1.");
            minSize = 0.1f;
        }
        if (maxSize < minSize)
        {
            Debug.LogWarning("GrassInstancingTest: maxSize < minSize. Clamping to minSize.");
            maxSize = minSize;
        }

        // Precompute transforms
        matrices = new Matrix4x4[instanceCount];
        for (int i = 0; i < instanceCount; i++)
        {
            // For 2D in XY plane: use (x, y, z=0). For 3D on ground plane XZ: use (x, y=groundY, z).
            Vector3 pos = new Vector3(
                Random.Range(-areaExtents.x, areaExtents.x),
                groundY,                                   // change to Random.Range(-areaExtents.y, areaExtents.y) if using XY
                Random.Range(-areaExtents.y, areaExtents.y)
            );

            float s = Random.Range(minSize, maxSize);      // uniform positive scale
            Vector3 scale = new Vector3(s, s, 1f);         // keep Z=1f to avoid zero-scale singularity for 2D quads


            Quaternion rot = Quaternion.Euler(Random.Range(-randomRot, randomRot), Random.Range(0f, 360f), Random.Range(-randomRot, randomRot));


            matrices[i] = Matrix4x4.TRS(pos, rot, scale);
        }
    }

    void Update()
    {
        // Draw all instances each frame
        Graphics.DrawMeshInstanced(grassMesh, 0, grassMaterial, matrices, instanceCount,
            null, ShadowCastingMode.Off, false, gameObject.layer);
    }
}
