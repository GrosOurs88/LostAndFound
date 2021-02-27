// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using UnityEditor;

public class SkyboxExtendedHub : EditorWindow
{
    string assetFolder = "Assets/BOXOPHOBIC/Atmospheric Height Fog";

    int assetVersion;
    string bannerVersion;

    static SkyboxExtendedHub window;

    [MenuItem("Window/BOXOPHOBIC/Skybox Cubemap Extended/Hub", false, 1070)]
    public static void ShowWindow()
    {
        window = GetWindow<SkyboxExtendedHub>(false, "Skybox Cubemap Extended", true);
        window.minSize = new Vector2(300, 200);
    }

    void OnEnable()
    {
        //Safer search, there might be many user folders
        string[] searchFolders;

        searchFolders = AssetDatabase.FindAssets("Skybox Cubemap Extended");

        for (int i = 0; i < searchFolders.Length; i++)
        {
            if (AssetDatabase.GUIDToAssetPath(searchFolders[i]).EndsWith("Skybox Cubemap Extended.pdf"))
            {
                assetFolder = AssetDatabase.GUIDToAssetPath(searchFolders[i]);
                assetFolder = assetFolder.Replace("/Skybox Cubemap Extended.pdf", "");
            }
        }

        bannerVersion = assetVersion.ToString();
        bannerVersion = bannerVersion.Insert(1, ".");
        bannerVersion = bannerVersion.Insert(3, ".");
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);

        EditorGUILayout.HelpBox("The included shader is compatible by default with Standard and Universal Render Pipelines!", MessageType.Info, true);

        GUILayout.Space(13);
        GUILayout.EndHorizontal();
    }
}


