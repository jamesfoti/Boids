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

	private void Start() {
		InitiatePositions();
	}

	public void InitiatePositions() {
		// Clear previous flock (if exists)
		if (this.boids.Count > 0) {
			ClearBoids();
		}

		// Create new flock
		for (int i = 0; i < this.size; i++) {
			CreateBoid();
		}
	}

	public void CreateBoid() {
		Color color = GenerateRandomColor();

		Vector3 pos = Random.insideUnitSphere * boidSpawnRadius;

		Boid boid = Instantiate(this.boidPrefab, parent: this.transform); // Parent = GameManager
		boid.transform.localPosition = pos;
		boid.transform.forward = Random.insideUnitSphere; // Direction
		boid.material.color = color;
		boid.flock = this;

		this.boids.Add(boid);
	
	}

	public void ClearBoids() {
		foreach (Boid boid in this.boids) {
			if (boid != null) {
				Destroy(boid.gameObject);
			}
		}
		this.boids.Clear();
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