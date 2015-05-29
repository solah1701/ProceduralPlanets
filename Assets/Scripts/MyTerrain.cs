using UnityEngine;
using System.Collections;

public class MyTerrain : MonoBehaviour
{
    public GameObject player;
    public int seed = 0;
    public int heightMin = 0;
    public int heightMax = 10;
    public int divMin = 12;
    public int divMax = 25;
    public Terrain selfTerrain;

    private float HM;
    private float divRange;
    private float terrainHeight;
    private float playerX;
    private float playerZ;

    void Init()
    {
        HM = Random.Range(heightMin, heightMax);
        divRange = Random.Range(divMin, divMax);
    }

    void Awake()
    {
        // Generate Terrain
        Init();
        GenerateTerrain(selfTerrain, HM);
    }

    // Use this for initialization
    void Start()
    {
        // Set player at terrain height
        playerX = player.transform.position.x;
        playerZ = player.transform.position.z;
        terrainHeight = selfTerrain.terrainData.GetHeight((int)playerX, (int)playerZ);
        player.transform.position.Set(playerX, terrainHeight + 1, playerZ);
    }

    // Our Generate Terrain Function
    public void GenerateTerrain(Terrain terrain, float tileSize)
    {
        // Heights for our Hills/Mountains
        var heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
        for (var i = 0; i < terrain.terrainData.heightmapWidth; i++)
        {
            for (var j = 0; j < terrain.terrainData.heightmapHeight; j++)
            {
                heights[i, j] = Mathf.PerlinNoise((i / (float)terrain.terrainData.heightmapWidth) * tileSize, (j / (float)terrain.terrainData.heightmapHeight) * tileSize) / divRange;

            }
        }
        Debug.LogWarning(string.Format("DivRange: {0}, H-Tiling: {1}", divRange, HM));
        terrain.terrainData.SetHeights(0, 0, heights);
    }
}
