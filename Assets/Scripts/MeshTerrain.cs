using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class MeshTerrain : MonoBehaviour
{
    public Vector3[] newVertices;
    public Vector2[] newUV;
    public int[] newTriangles;

    // Use this for initialization
    void Start()
    {
        CreateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateMesh(bool clear = false)
    {
        var mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        if (clear) mesh.Clear();
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
        mesh.name = name;
    }

    private void UpdateMesh()
    {
        CreateMesh(true);
    }

    private void ModifyMesh()
    {
        var mesh = GetComponent<MeshFilter>().mesh;
        var vertices = mesh.vertices;
        var normals = mesh.normals;
        var i = 0;
        while (i < vertices.Length)
        {
            vertices[i] += normals[i] * Mathf.Sin(Time.time);
            i++;
        }
        mesh.vertices = vertices;
    }
}
