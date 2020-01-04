using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainManager))]
public class TerrainMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TerrainManager terrainManager = (TerrainManager)target; 

        if (GUILayout.Button("GenerateMap"))
        {
            Debug.Log("Map Generated");

            terrainManager.ClearMap();
            terrainManager.GenerateMap();
        }

        if (GUILayout.Button("ClearMap"))
        {
            terrainManager.ClearMap();
        }
        if (DrawDefaultInspector())
        {
            terrainManager.ClearMap();
            terrainManager.GenerateMap();
        }



        
    }
}
