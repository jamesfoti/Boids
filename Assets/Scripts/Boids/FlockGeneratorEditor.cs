using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FlockGenerator))]
public class FlockUnityEditor : Editor {
    private bool isAbleCreateFlock = false;

    public override void OnInspectorGUI() {
        FlockGenerator flockGenerator = (FlockGenerator)target;
        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (!isAbleCreateFlock) {
            EditorGUILayout.HelpBox("Unity editor must be in play mode before spawning flocks!", MessageType.Warning, wide: true);
        }

        if (GUILayout.Button("Generate Flock") && EditorApplication.isPlaying) {
            flockGenerator.CreateFlock();
            isAbleCreateFlock = true;
        }
        else {
            isAbleCreateFlock = false;
        }

        if (GUILayout.Button("Clear Flocks")) {
            for (int i = 0; i < flockGenerator.flocks.Count; i++) {
                flockGenerator.ClearFlocks();
            }
            
        }
    }

}