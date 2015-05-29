using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class Generator : MonoBehaviour
{
    protected MeshData meshData;

    protected void CreateMesh(bool clear = false)
    {
        CreateMesh(meshData, name, clear);
    }

    protected void CreateMesh(MeshData data, string dataName, bool clear)
    {
        var mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        if (clear) mesh.Clear();
        mesh.vertices = data.newVertices;
        mesh.uv = data.newUV;
        mesh.triangles = data.newTriangles;
        mesh.name = dataName;
    }

    protected void UpdateMesh()
    {
        CreateMesh(true);
    }

    protected void ModifyMesh()
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

    protected int[] CreateTriangles(int samples, bool reversed = true)
    {
        var p = 0;
        var index = 0;
        var triangles = new int[(samples - 1) * (samples - 1) * 6];
        for (var i = 0; i < samples - 1; i++)
        {
            for (var j = 0; j < samples - 1; j++)
            {
                triangles[p++] = index;
                triangles[p++] = index + (reversed ? samples : 1);
                triangles[p++] = index + (reversed ? 1 : samples);
                triangles[p++] = index + samples;
                triangles[p++] = index + (reversed ? samples + 1 : 1);
                triangles[p++] = index++ + (reversed ? 1 : samples + 1);
            }
            index++;
        }
        return triangles;
    }
}
