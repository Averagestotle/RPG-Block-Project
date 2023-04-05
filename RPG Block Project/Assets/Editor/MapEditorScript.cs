using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEditor.TerrainTools;

[CustomEditor(typeof (MapGeneratorScript))]
public class MapEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGeneratorScript mapGenerator = target as MapGeneratorScript;

        if (GUILayout.Button("Generate Seed"))
        {
            System.Random r = new System.Random();

            mapGenerator.GenerateNewSeedValue(r.Next());
        }

        if (GUILayout.Button("Generate Map"))
        {
            mapGenerator.GenerateMap();
        }

        //mapGenerator.GenerateMap();
    }


}
