using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
public class FlockGeneratorWindowEditor : EditorWindow {
	// First section (see OnGUI() below)
	public int startingFlockCount;

	public static FlockGenerator flockGenerator;
	private Flock flockPrefab;

	[MenuItem("Window/Boids")]
	public static void ShowWindow() {
        GetWindow<FlockGeneratorWindowEditor>("Boids");
	}

	public void Awake() {
		flockGenerator = (FlockGenerator)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Boids/FlockGenerator.prefab", typeof(FlockGenerator));
		flockPrefab = (Flock)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Boids/Flock.prefab", typeof(Flock));
		Instantiate(flockGenerator);
	}

	private void OnGUI() {
		// Internally, check to make sure the Flock.cs prefab is connected
		if (flockGenerator == null) {
			flockGenerator = (FlockGenerator)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Boids/FlockGenerator.prefab", typeof(FlockGenerator));
		}

		if (flockPrefab == null) {
			flockPrefab = (Flock)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Boids/Flock.prefab", typeof(Flock));
		}

		if (flockPrefab.boidPrefab == null) {
			EditorGUILayout.HelpBox("A boid model/prefab must be attached first.", MessageType.Error);
		}

		// First section
		EditorGUILayout.LabelField("Flock Generation Settings", EditorStyles.boldLabel);
		FlockGenerator.flockSpawnCount = EditorGUILayout.IntSlider("Start() Flock Count", FlockGenerator.flockSpawnCount, 0, 100);
		EditorGUILayout.Space();

		// Second section
		EditorGUILayout.LabelField("Flock Settings", EditorStyles.boldLabel);
		flockPrefab.boidPrefab = EditorGUILayout.ObjectField("Boid Prefab", flockPrefab.boidPrefab, typeof(Boid), true) as Boid;
		flockPrefab.size = EditorGUILayout.IntSlider("Flock Size", flockPrefab.size, 0, 100);
		flockPrefab.boidSpawnRadius = EditorGUILayout.Slider("Boid Spawn Radius", flockPrefab.boidSpawnRadius, 0f, 100f);
		EditorGUILayout.Space();

	
		if (GUILayout.Button("Generate Flock")) {
			Debug.Log("Spawn Flock");
			flockGenerator.CreateFlock(flock: flockPrefab, numBoids: flockPrefab.size);
		}

		if (GUILayout.Button("Clear All Flocks")) {
			Debug.Log("Clear");
			flockGenerator.ClearFlocks();
		}
	}
}*/