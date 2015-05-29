using UnityEngine;
using System.Collections;

public struct MeshData
{
    public Vector3[] newVertices;
    public Vector2[] newUV;
    public int[] newTriangles;

    public MeshData(int samples)
    {
        newVertices = new Vector3[samples * samples];
        newUV = new Vector2[samples * samples];
        newTriangles = new int[(samples - 1) * (samples - 1) * 6];
    }
}
