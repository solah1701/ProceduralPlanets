using UnityEngine;
using System.Collections;

public class FractalPlanet : MonoBehaviour
{

    public float octaves;
    public float frequency;
    public float amplitude;
    public float lacunarity;
    public float gain;
    public int hgrid;
    public Terrain theTerrain;

    private float[][] map;

    void FractalBrownianMotion(int x, int y)
    {
        var total = 0f;
        frequency = 1.0f/(float) hgrid;
        amplitude = gain;
        for (var i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise((float)x * frequency, (float)y * frequency) * amplitude;
            frequency *= lacunarity;
            amplitude *= gain;
        }
        map[x][y] = total;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
