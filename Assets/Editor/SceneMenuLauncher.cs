using UnityEngine;
using UnityEditor;
using System.Collections;

public class SceneMenuLauncher : MonoBehaviour
{

    [MenuItem("Scenes/Planet")]
    static void OpenPlanet()
    {
        OpenScene("Scenes/Planet.unity");
    }

    [MenuItem("Scenes/Main")]
    static void OpenMain()
    {
        OpenScene("Scenes/Main.unity");
    }

    [MenuItem("Scenes/Terrain 1")]
    static void OpenTerrain1()
    {
        OpenScene("Scenes/Terrain1.unity");
    }

    [MenuItem("Scenes/Terrain 2")]
    static void OpenTerrain2()
    {
        OpenScene("Scenes/Terrain2.unity");
    }

    [MenuItem("Scenes/Test")]
    static void OpenTest()
    {
        OpenScene("Scenes/Test.unity");
    }

    static void OpenScene(string path)
    {
        EditorApplication.SaveCurrentSceneIfUserWantsTo();
        EditorApplication.OpenScene(string.Format("Assets/{0}", path));
    }
}
