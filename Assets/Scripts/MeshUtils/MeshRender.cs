using UnityEngine;
using System.Collections;

public class MeshRender : MonoBehaviour
{
    protected virtual void CreateMesh(bool clear = false)
    {
        var mesh = new Mesh { name = name };
        GetComponent<MeshFilter>().mesh = mesh;
        if (clear) mesh.Clear();
    }
}
