using UnityEngine;

public class Island
{
    public Vector3 Center { get; set; }
    public float Radius { get; set; }

    public Island(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }
}
