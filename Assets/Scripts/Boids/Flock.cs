using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Flock : MonoBehaviour {

	public Boid boidPrefab; 
	public int size;
	public float boidSpawnRadius;
	public Vector3 globalCenter; // Used for determining boundaries of flock movement
	public float globalRadius; // Used for determining boundaries of flock movement\

	public List<Boid> boids = new List<Boid>();
	
	[HideInInspector]

	public void InitiatePositions() {
		// Clear previous flock (if exists)
		

		// Create new flock
		
	}

	public void CreateBoids(int count = 1) {
		Color color = GenerateRandomColor();

		for (int i = 0; i < count; i ++) {
			Vector3 pos = Random.insideUnitSphere * boidSpawnRadius;
			Boid boid = Instantiate(boidPrefab, parent: this.transform); // Parent = GameManager
			boid.transform.localPosition = pos;
			boid.transform.forward = Random.insideUnitSphere; // Direction
			boid.flock = this;

			// Change color  (for more info: https://answers.unity.com/questions/283271/material-leak-in-editor.html)
			var tempMaterial = new Material(boid.GetComponent<Renderer>().sharedMaterial);
			tempMaterial.color = color;
			boid.GetComponent<Renderer>().sharedMaterial = tempMaterial;

			boids.Add(boid);
		}
	}

	public void ClearBoids() {
		foreach (Boid boid in boids) {
			if (boid != null) {
				Destroy(boid.gameObject);
			}
		}
		boids.Clear();
	}

	private Color GenerateRandomColor() {
		Color color = new Color(
		  Random.Range(0f, 1f),
		  Random.Range(0f, 1f),
		  Random.Range(0f, 1f)
		);

		return color;
	}



}