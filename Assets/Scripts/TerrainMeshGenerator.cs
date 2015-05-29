using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TerrainMeshGenerator
{
    public static void FillMesh(ref Mesh meshToUpdate, float[, ,] data, int size, int height, float surfaceCrossValue)
    {
        var vertexIndex = 0;
        var interpolatedValues = new Vector3[12];

        var vertices = new List<Vector3>();
        var triangleIndices = new List<int>();

        for (var x = 0; x < size - 1; x++)
            for (var y = 0; y < height - 1; y++)
                for (var z = 0; z < size - 1; z++)
                {
                    if (vertices.Count > 64000) break;
                    var basePoint = new Vector3(x, y, z);
                    var p0 = data[x, y, z];
                    var p1 = data[x + 1, y, z];
                    var p2 = data[x, y + 1, z];
                    var p3 = data[x + 1, y + 1, z];
                    var p4 = data[x, y, z + 1];
                    var p5 = data[x + 1, y, z + 1];
                    var p6 = data[x, y + 1, z + 1];
                    var p7 = data[x + 1, y + 1, z + 1];

                    var crossBitMap = 0;
                    if (p0 < surfaceCrossValue) crossBitMap |= 1;
                    if (p1 < surfaceCrossValue) crossBitMap |= 2;
                    if (p2 < surfaceCrossValue) crossBitMap |= 8;
                    if (p3 < surfaceCrossValue) crossBitMap |= 4;
                    if (p4 < surfaceCrossValue) crossBitMap |= 16;
                    if (p5 < surfaceCrossValue) crossBitMap |= 32;
                    if (p6 < surfaceCrossValue) crossBitMap |= 128;
                    if (p7 < surfaceCrossValue) crossBitMap |= 64;

                    var edgeBits = Contouring3D.EdgeTableLookup[crossBitMap];

                    if (edgeBits == 0) continue;
                    float interpolatedCrossingPoint;
                    if ((edgeBits & 1) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p0) / (p1 - p0);
                        interpolatedValues[0] = Vector3.Lerp(new Vector3(x, y, z), new Vector3(x + 1, y, z),
                                                             interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 2) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p1) / (p3 - p1);
                        interpolatedValues[1] = Vector3.Lerp(new Vector3(x + 1, y, z), new Vector3(x + 1, y + 1, z),
                                                             interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 4) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p2) / (p3 - p2);
                        interpolatedValues[2] = Vector3.Lerp(new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z),
                                                             interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 8) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p0) / (p2 - p0);
                        interpolatedValues[3] = Vector3.Lerp(new Vector3(x, y, z), new Vector3(x, y + 1, z),
                                                             interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 16) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p4) / (p5 - p4);
                        interpolatedValues[4] = Vector3.Lerp(new Vector3(x, y, z + 1), new Vector3(x + 1, y, z + 1),
                                                             interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 32) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p5) / (p7 - p5);
                        interpolatedValues[5] = Vector3.Lerp(new Vector3(x + 1, y, z + 1), new Vector3(x + 1, y + 1, z + 1),
                                                             interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 64) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p6) / (p7 - p6);
                        interpolatedValues[6] = Vector3.Lerp(new Vector3(x, y + 1, z + 1), new Vector3(x + 1, y + 1, z + 1),
                                                             interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 128) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p4) / (p6 - p4);
                        interpolatedValues[7] = Vector3.Lerp(new Vector3(x, y, z + 1), new Vector3(x, y + 1, z + 1),
                                                             interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 256) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p0) / (p4 - p0);
                        interpolatedValues[8] = Vector3.Lerp(new Vector3(x, y, z), new Vector3(x, y, z + 1),
                                                             interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 512) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p1) / (p5 - p1);
                        interpolatedValues[9] = Vector3.Lerp(new Vector3(x + 1, y, z), new Vector3(x + 1, y, z + 1),
                                                             interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 1024) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p3) / (p7 - p3);
                        interpolatedValues[10] = Vector3.Lerp(new Vector3(x + 1, y + 1, z), new Vector3(x + 1, y + 1, z + 1),
                                                             interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 2048) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p2) / (p6 - p2);
                        interpolatedValues[11] = Vector3.Lerp(new Vector3(x, y + 1, z), new Vector3(x, y + 1, z + 1),
                                                             interpolatedCrossingPoint);
                    }
                    //Debug.Log(string.Format("crossBitMap {0}, triangleIndex {1}", crossBitMap, 0));
                    //crossBitMap <<= 4;
                    var triangleIndex = 0;
                    //Debug.Log(string.Format("crossBitMap {0}, triangleIndex {1}", crossBitMap, triangleIndex));
                    while (Contouring3D.TriangleLookupTable[crossBitMap, triangleIndex] != -1)
                    {
                        int index1 = Contouring3D.TriangleLookupTable[crossBitMap, triangleIndex];
                        int index2 = Contouring3D.TriangleLookupTable[crossBitMap, triangleIndex + 1];
                        int index3 = Contouring3D.TriangleLookupTable[crossBitMap, triangleIndex + 2];

                        vertices.Add(new Vector3(interpolatedValues[index1].x, interpolatedValues[index1].y, interpolatedValues[index1].z));
                        vertices.Add(new Vector3(interpolatedValues[index2].x, interpolatedValues[index2].y, interpolatedValues[index2].z));
                        vertices.Add(new Vector3(interpolatedValues[index3].x, interpolatedValues[index3].y, interpolatedValues[index3].z));

                        triangleIndices.Add(vertexIndex);
                        triangleIndices.Add(vertexIndex + 1);
                        triangleIndices.Add(vertexIndex + 2);
                        vertexIndex += 3;
                        triangleIndex += 3;
                        //Debug.Log(string.Format("crossBitMap {0}, triangleIndex {1}", crossBitMap, triangleIndex));
                    }
                }
        var texCoords = new List<Vector2>();
        var emptyTexCoords0 = new Vector2(0, 0);
        var emptyTexCoords1 = new Vector2(0, 1);
        var emptyTexCoords2 = new Vector2(1, 1);

        for (var texturePointer = 0; texturePointer < vertices.Count; texturePointer += 3)
        {
            texCoords.Add(emptyTexCoords1);
            texCoords.Add(emptyTexCoords2);
            texCoords.Add(emptyTexCoords0);
        }

        meshToUpdate.Clear();
        meshToUpdate.vertices = vertices.ToArray();
        meshToUpdate.triangles = triangleIndices.ToArray();
        meshToUpdate.uv = texCoords.ToArray();
        meshToUpdate.RecalculateNormals();
        meshToUpdate.RecalculateBounds();
    }

}
