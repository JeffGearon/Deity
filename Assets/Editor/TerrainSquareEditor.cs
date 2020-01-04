using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainSquare))]
public class NewBehaviourScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TerrainSquare terrainSquare = (TerrainSquare) target;

        if (GUILayout.Button("Set Terrain Square"))
        {
            terrainSquare.AssignTerrainType();
        }
    }
}
