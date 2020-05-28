using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FlockGenerator))]
public class FlockUnityEditor : Editor {
    public override void OnInspectorGUI() {
        FlockGenerator flockGenerator = (FlockGenerator)target;
        DrawDefaultInspector();

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Flock")) {
            flockGenerator.CreateFlock(flockGenerator.flockPrefab, flockGenerator.flockPrefab.size);
        }

        if (GUILayout.Button("Clear Flocks")) {
            for (int i = 0; i < flockGenerator.flocks.Count; i++) {
                flockGenerator.ClearFlocks();
            }
            
        }
    }

}