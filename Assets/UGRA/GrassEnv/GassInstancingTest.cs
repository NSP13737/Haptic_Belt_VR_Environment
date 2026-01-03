
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Drawing;

public class GrassInstancingTest : MonoBehaviour
{
    [Header("Assets")]
    public Mesh grassMesh;            // Should be a 2D quad or your grass mesh (submesh 0 must have triangles)
    public Material grassMaterial;    // Shader must support GPU Instancing and have it enabled

    [Header("Placement")]
    [SerializeField] private int instanceCount = 512; // <= 1023 per batch
    [SerializeField] private float areaRadius = 10f;
    [SerializeField] private float groundY = 0f; // If using XZ plane with Y as up

    [Header("Variation")]
    [SerializeField] private float minSize = 0.6f;  // Ensure > 0
    [SerializeField] private float maxSize = 1.4f;  // Ensure >= minSize
    [SerializeField] private float heightScalar = 1f;
    [SerializeField] private float randomRot = 15f;
    [SerializeField] private float weightDistribution = 1f;




    private readonly List<Matrix4x4[]> _batches = new List<Matrix4x4[]>();

    private struct Settings
    {
        private int _instanceCount;
        private float _areaRadius;
        private float _groundY;
        private float _minSize;
        private float _maxSize;
        private float _heightScalar;
        private float _randomRot;
        private float _weightDistribution;

        public Settings(int instanceCount, float areaRadius, float groundY, float minSize, float maxSize, float heightScalar, float randomRot, float weightDistribution)
        {
            _instanceCount = instanceCount;
            _areaRadius = areaRadius;
            _groundY = groundY;
            _minSize = minSize;
            _maxSize = maxSize;
            _heightScalar = heightScalar;
            _randomRot = randomRot;
            _weightDistribution = weightDistribution;
        }

        public static bool operator !=(Settings s1, Settings s2)
        {
            return ((s1._weightDistribution != s2._weightDistribution) || (s1._instanceCount != s2._instanceCount) || (s1._areaRadius != s2._areaRadius) || (s1._groundY != s2._groundY ) || (s1._minSize != s2._minSize) || (s1._maxSize != s2._maxSize) || (s1._heightScalar != s2._heightScalar) || (s1._randomRot != s2._randomRot));
        }
        public static bool operator ==(Settings s1, Settings s2)
        {
            return ((s1._weightDistribution == s2._weightDistribution) && (s1._instanceCount == s2._instanceCount) && (s1._areaRadius == s2._areaRadius) && (s1._groundY == s2._groundY) && (s1._minSize == s2._minSize) && (s1._maxSize == s2._maxSize) && (s1._heightScalar == s2._heightScalar) && (s1._randomRot == s2._randomRot));
        }
    }

    private Settings previousSettings;
    void Awake()
    {
        previousSettings = new Settings(instanceCount, areaRadius, groundY, minSize, maxSize, heightScalar, randomRot, weightDistribution);

        if (!validateInputs()) return;

        BuildBatches();

    }
    private bool validateInputs()
    {
        // Basic validation
        if (grassMesh == null)
        {
            Debug.LogError("GrassInstancingTest: grassMesh is not assigned.");
            enabled = false;
            return false;
        }
        if (grassMaterial == null)
        {
            Debug.LogError("GrassInstancingTest: grassMaterial is not assigned.");
            enabled = false;
            return false;
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
        return true;
    }

    private void BuildBatches() 
    {
        _batches.Clear();

        const int MaxPerBatch = 1023;
        int remainingInstances = instanceCount;

        while (remainingInstances > 0)
        {
            int countThisBatch = Mathf.Min(remainingInstances, MaxPerBatch);
            var matricies = new Matrix4x4[countThisBatch];

            for (int i = 0; i < countThisBatch; i++)
            {
                float weight = weightDistribution * ((Random.value + Random.value + Random.value) / 3.0f);
                Vector2 p = Random.insideUnitCircle * (areaRadius*weight);
                Vector3 pos = new Vector3(p.x, groundY, p.y);
                float s = Random.Range(minSize, maxSize);
                Vector3 scale = new Vector3(s, s*heightScalar, s);
                Quaternion rot = Quaternion.Euler(Random.Range(-randomRot, randomRot), Random.Range(0f,360f), Random.Range(-randomRot, randomRot));
                matricies[i] = Matrix4x4.TRS(pos, rot, scale);
            }
            _batches.Add(matricies);
            remainingInstances -= countThisBatch;
        }

    }

    void Update()
    {
        Settings currentSettings = new Settings(instanceCount, areaRadius, groundY, minSize, maxSize, heightScalar, randomRot, weightDistribution);
        if (currentSettings != previousSettings)
        {
            Awake(); //recompute batches if we change vars in editor
        }

        // Draw all instances each frame
        for (int i = 0; i < _batches.Count; i++)
        {
            var matrices = _batches[i];
            Graphics.DrawMeshInstanced(
                grassMesh,
                submeshIndex: 0,
                grassMaterial,
                matrices,
                matrices.Length,
                properties: null,
                castShadows: ShadowCastingMode.Off,
                receiveShadows: false,
                layer: gameObject.layer);
        }
    }
}
