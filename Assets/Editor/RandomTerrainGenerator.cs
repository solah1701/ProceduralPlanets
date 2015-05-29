using UnityEngine;
using UnityEditor;

public class RandomTerrainGenerator : ScriptableWizard
{

    private static Random random = new Random(0);
    // The higher the numbers, the more hills/mountains there are
    private float HM = Random.Range(0, 10);

    // The lower the numbers in the number range, the higher the hills/mountains will be...
    private float divRange = Random.Range(12, 25);

    [MenuItem("Terrain/Generate Random Terrain")]
    public static void CreateWizard(MenuCommand command)
    {
        DisplayWizard("Generate Random Terrain", typeof(RandomTerrainGenerator));
    }

    void OnWizardCreate()
    {
        var gameObject = Selection.activeGameObject;
        if (gameObject.GetComponent<Terrain>())
            GenerateTerrain(gameObject.GetComponent<Terrain>(), HM);
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
