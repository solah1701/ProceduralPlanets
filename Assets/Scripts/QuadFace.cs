using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class QuadFace : Generator
{
    //[HideInInspector]
    public int samples = 16;

    [Header("Which Face am I")]
    public QuadCube.FaceType face;

    // Use this for initialization
    void Start()
    {
        Main();
    }

    // Update is called once per frame
    void Update()
    {
        Main();
    }

    bool Validate()
    {
        return samples >= 1 && samples <= 254;
    }

    private void Main()
    {
        if (!Validate()) return;
        var edge = 1f / (samples - 1);
        BuildMesh(edge, face);
        CreateMesh();
    }

    protected void BuildMesh(float edge, QuadCube.FaceType side)
    {
        // Create the vertices and texture coords
        meshData = new MeshData(samples);

        // Fill in the data
        for (int i = 0, p = 0; i < samples; i++)
        {
            for (var j = 0; j < samples; j++)
            {
                // Current vertex
                var x = 2f * i * edge - 1;
                var z = 2f * j * edge - 1;
                // Height of this vertex (from heightmap)
                const float height = 1f;
                if (side == QuadCube.FaceType.Top)
                {
                    meshData.newVertices[p] = SphereVertex(-x, height, z);
                    meshData.newUV[p++] = new Vector2(1 - i / (samples - 1f), 1 - j / (samples - 1f));
                }
                if (side == QuadCube.FaceType.Bottom)
                {
                    meshData.newVertices[p] = SphereVertex(x, -height, z);
                    meshData.newUV[p++] = new Vector2(i / (samples - 1f), j / (samples - 1f));
                }
                if (side == QuadCube.FaceType.Front)
                {
                    meshData.newVertices[p] = SphereVertex(x, z, height);
                    meshData.newUV[p++] = new Vector2(i / (samples - 1f), j / (samples - 1f));
                }
                if (side == QuadCube.FaceType.Back)
                {
                    meshData.newVertices[p] = SphereVertex(-x, z, -height);
                    meshData.newUV[p++] = new Vector2(i / (samples - 1f), j / (samples - 1f));
                }
                if (side == QuadCube.FaceType.Left)
                {
                    meshData.newVertices[p] = SphereVertex(-height, x, z);
                    meshData.newUV[p++] = new Vector2(j / (samples - 1f), i / (samples - 1f));
                }
                if (side == QuadCube.FaceType.Right)
                {
                    meshData.newVertices[p] = SphereVertex(height, -x, z);
                    meshData.newUV[p++] = new Vector2(1 - j / (samples - 1f), 1 - i / (samples - 1f));
                }
            }
        }
        meshData.newTriangles = CreateTriangles(samples, !(side == QuadCube.FaceType.Left || side == QuadCube.FaceType.Right));
    }

    private static Vector3 SphereVertex(float x, float y, float z)
    {
        var dx = x * Mathf.Sqrt(1f - (y * y / 2f) - (z * z / 2f) + (y * y * z * z / 3f));
        var dy = y * Mathf.Sqrt(1f - (z * z / 2f) - (x * x / 2f) + (z * z * x * x / 3f));
        var dz = z * Mathf.Sqrt(1f - (x * x / 2f) - (y * y / 2f) + (x * x * y * y / 3f));
        return new Vector3(dx, dy, dz);
    }
}
