using System.Linq;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class QuadCube : MonoBehaviour
{
    public int samples = 16;

    private GameObject[] children;

    public enum FaceType
    {
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back
    }

    // Use this for initialization
    void Start()
    {
        Main();
    }

    // Update is called once per frame
    void Update()
    {
        Main();
        UpdateChildObjects();
    }

    void OnDestroy()
    {
        if (children == null) return;
        for (var i = 5; i >= 0; i--)
        {
            DestroyImmediate(children[i]);
        }
    }

    bool Validate()
    {
        return samples >= 1 && samples <= 254;
    }

    private void Main()
    {
        if (!Validate()) return;
        if (children != null) return;
        children = new GameObject[6];
        children[0] = ReattachChildObject(FaceType.Top);
        children[1] = ReattachChildObject(FaceType.Bottom);
        children[2] = ReattachChildObject(FaceType.Left);
        children[3] = ReattachChildObject(FaceType.Right);
        children[4] = ReattachChildObject(FaceType.Back);
        children[5] = ReattachChildObject(FaceType.Front);
    }

    private GameObject ReattachChildObject(FaceType face)
    {
        var obj = transform.FindChild(face.ToString());
        return obj == null ? AddChildObject(face) : obj.gameObject;
    }

    private void UpdateChildObjects()
    {
        for (var i = 0; i < 6; i++)
        {
            if (children.Count() <= i) return;
            var script = children[i].GetComponent<QuadFace>();
            script.samples = samples + 1;
        }
    }

    private GameObject AddChildObject(FaceType objectName)
    {
        var obj = new GameObject(objectName.ToString());
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        var script = obj.AddComponent<QuadFace>();
        script.face = objectName;
        script.samples = samples + 1;
        return obj;
    }
}
