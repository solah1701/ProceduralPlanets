using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProceduralTerrain : MonoBehaviour
{
   // private int size = 24;
    public int height = 20;
    public int width = 30;
    public int depth = 30;

    public ComputeShader marchingCubes;

    private float[, ,] data;

    // When an edge transitions between a positive and negative value, it'll be marked as "crossed"
    public float surfaceCrossValue = 0;

    // The scale of the noise for input into the system
    public float noiseScaleFactor = 20;

    private Mesh localMesh;
    
    private MeshFilter meshFilter;

    // Use this for initialization
    void Start()
    {
        localMesh = new Mesh {name = name};
        meshFilter = GetComponent<MeshFilter>();
        data = new float[width, height, depth];
        FillData(transform.position.x, transform.position.y, transform.position.z);
        ApplyDataToMesh();
    }

    // Update is called once per frame
    void Update()
    {
        var changed = false;
        var changedMeshOnly = false;
        if (Input.GetKey(KeyCode.Q))
        {
            surfaceCrossValue += .01f;
            changedMeshOnly = true;
        }
        if (Input.GetKey(KeyCode.E))
        {
            surfaceCrossValue -= .01f;
            changedMeshOnly = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.Translate(-.5f, 0, 0, Space.World);
            transform.Translate(-.5f, 0, 0, Space.World);
            changed = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.Translate(.5f, 0, 0, Space.World);
            transform.Translate(.5f, 0, 0, Space.World);
            changed = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.Translate(0, 0, -.5f, Space.World);
            transform.Translate(0, 0, -.5f, Space.World);
            changed = true;
        }
        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.Translate(0, 0, .5f, Space.World);
            transform.Translate(0, 0, .5f, Space.World);
            changed = true;
        }
        if (Input.GetKey(KeyCode.R))
        {
            noiseScaleFactor += .1f;
            changed = true;
        }
        if (Input.GetKey(KeyCode.F))
        {
            noiseScaleFactor -= .1f;
            changed = true;
        }
        if (!changed && !changedMeshOnly) return;
        if (changed)
            FillData(transform.position.x, transform.position.y, transform.position.z);
        ApplyDataToMesh();
    }

    void ApplyDataToMesh()
    {
        TerrainMeshGenerator.FillMesh(ref localMesh, data, width, height, surfaceCrossValue);
        meshFilter.mesh = localMesh;
    }

    void FillData(float xOrigin, float yOrigin, float zOrigin)
    {
        for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                for (var z = 0; z < depth; z++)
                {
                    if (x == 0 || x == width - 1)
                    {
                        data[x, y, z] = -1;
                        continue;
                    }
                    if (y == 0 || y == height - 1)
                    {
                        data[x, y, z] = -1;
                        continue;
                    }
                    if (z == 0 || z == depth - 1)
                    {
                        data[x, y, z] = -1;
                        continue;
                    }
                    var dataX = (xOrigin + x) / noiseScaleFactor;
                    var dataY = (yOrigin + y) / noiseScaleFactor;
                    var dataZ = (zOrigin + z) / noiseScaleFactor;

                    data[x, y, z] = Mathf.PerlinNoise(dataY, dataX + dataZ) - Mathf.PerlinNoise(dataX, dataZ);
                    data[x, y, z] += -(((float)y / height) - .5f);
                }
    }
}
