using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{
    private Vector3[] newVertices;
    private Vector2[] newUV;
    private int[] newTriangles;
    public float width;
    public int samples;
    public int perlinLevels;

    public int count;
    public float size;
    public float density = 0.1f;
    public float separation = -0.2f;
    public float terrainHeight = 1f;
    public int seed;

    private Random random;
    private Island[] islands;
    private float[] heights;

    // Use this for initialization
    void Start()
    {
        Main();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        Main();
#endif
    }

    bool Validate()
    {
        if (size <= 0) return false;
        if (count < 1) return false;
        if (samples < 1 || samples > 255) return false;
        if (perlinLevels < 1) return false;
        return !(width < 1);
    }
    private void Main()
    {
        if (!Validate()) return;
        random = new Random(seed);
        islands = GenerateIslands(count, size, density, separation);
        heights = new float[islands.Count()];

        for (var i = 0; i < heights.Length; i++)
        {
            heights[i] = random.RandomUniform(0, 1) * islands[i].Radius * terrainHeight;
        }
        DoStuff();
        CreateMesh();
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

    public void DoStuff()
    {
        float edge = width / (samples - 1);
        float offset = width / 2;

        // Create the vertices and texture coords
        newVertices = new Vector3[samples * samples];
        newUV = new Vector2[samples * samples];
        newTriangles = new int[(samples - 1) * (samples - 1) * 6];

        // Fill in the data
        for (int i = 0, p = 0; i < samples; i++)
        {
            for (var j = 0; j < samples; j++)
            {
                // Current vertex
                var center = new Vector3(i * edge - offset, 0, j * edge - offset);
                // Height of this vertex (from heightmap)
                var h = SampleGaussians(center.x, center.z);
                center.y = h;
                newVertices[p] = center;
                // UV coords in [0,1] space
                newUV[p++] = new Vector2(i / (samples - 1f), j / (samples - 1f));
            }
        }
        CreateTriangles();
        GetComponent<MeshRenderer>().sharedMaterial.color = Color.green;
    }

    private void CreateTriangles()
    {
        var p = 0;
        var index = 0;
        for (var i = 0; i < samples - 1; i++)
        {
            for (var j = 0; j < samples - 1; j++)
            {
                newTriangles[p++] = index;
                newTriangles[p++] = index + 1;
                newTriangles[p++] = index + samples;
                newTriangles[p++] = index + samples;
                newTriangles[p++] = index + 1;
                newTriangles[p++] = index++ + samples + 1;
            }
            index++;
        }
    }

    private Island[] GenerateIslands(int count, float size, float density, float separation)
    {
        var islands = new List<Island>();
        for (var i = 0; i < count; i++)
        {
            for (var j = 0; j < count; j++)
            {
                if (!random.RandomBernoulli(density)) continue;
                var x = i * size + random.RandomUniform(0, size - size * separation) - size * count / 2;
                var z = j * size + random.RandomUniform(0, size - size * separation) - size * count / 2;
                var island = new Island(new Vector3(x, 0, z), 0);
                islands.Add(island);
            }
        }

        var cannotGrow = new bool[islands.Count];
        var canGrow = islands.Count;

        while (canGrow > 0)
        {
            for (var i = 0; i < islands.Count; i++)
            {
                var child = islands[i];
                if (cannotGrow[i]) continue;
                if (islands.Where((t, j) => i != j).Any(other => Vector3.Distance(child.Center, other.Center) - size * separation < (child.Radius + other.Radius)))
                {
                    cannotGrow[i] = true;
                    canGrow--;
                }
                if (!cannotGrow[i]) child.Radius += size * 0.01f;
            }
        }
        return islands.ToArray();
    }

    private float SamplePerlin(float x, float y)
    {
        var w = width / 10f;
        var result = 0f;
        for (var i = 0; i < perlinLevels; i++)
        {
            result += (Mathf.PerlinNoise(x / w, y / w)) * w / 2;
            w /= 10;
        }
        return result;
    }

    private float SampleHeight(float x, float y)
    {
        return Mathf.PerlinNoise(x, y);
    }

    private float SampleHeight(float x, float y, Island island, float height)
    {
        var d = Vector3.Distance(new Vector3(x, 0, y), island.Center);
        var mountain = Mathf.Exp(-d * d / (island.Radius * island.Radius)) * height;

        return mountain;
    }

    private float SampleGaussians(float x, float y)
    {
        float h = 0;

        for (int i = 0; i < islands.Length; i++)
        {
            h += SampleHeight(x, y, islands[i], heights[i]);
        }

        return h;
    }
}
