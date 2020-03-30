using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {
	public Boid prefab; // Edit in the editor

	[HideInInspector] // I usually hide things that don't need to be in the inspector
	public List<Boid> boids = new List<Boid>();

	// Edit these in the editor
	public float spawnRadius;
	public int defaultFlockSize;
	public Vector3 globalCenter;
	public float globalRadius;
	public SimulationUI simUI;

	private Vector3 randomPosition; // Used for "Lazy Flight"

	private void Start() {
		randomPosition = GenerateRandomPoint();
		InitiatePositions();
	}

	private void Update() {
		if (simUI.isMoving) {
			MoveBoids();
		}
	}

	public void InitiatePositions() {
		int flockSize = (int)simUI.flockSizeSlider.value;

		// Clear previous flock (if exists)
		if (boids.Count > 0) {
			ClearBoids();
		}

		// Create new flock
		for (int i = 0; i < flockSize; i++) {
			CreateBoid();
		}
	}

	public void CreateBoid() {
		Vector3 pos = Random.insideUnitSphere * spawnRadius;
		Boid boid = Instantiate(prefab, parent: this.transform); // Parent = GameManager
		boid.transform.localPosition = pos;
		boid.transform.forward = Random.insideUnitSphere; // Direction
		boids.Add(boid);
	}

	private void ClearBoids() {
		for (int i = 0; i < boids.Count; i++) {
			Destroy(boids[i].gameObject);
		}
		boids.Clear();
	}

	public void MoveBoids() {
		// vectors based off different behaviors and tweaks
		Vector3 cohesionMove, alignmentMove, separationMove, steeredCohesionMove;
		Vector3 stayBoundedMove, strongWind, tendToPlaceMove, sphericalMove;

		cohesionMove = Vector3.zero;
		alignmentMove = Vector3.zero;
		separationMove = Vector3.zero;
		steeredCohesionMove = Vector3.zero;
		stayBoundedMove = Vector3.zero;
		strongWind = Vector3.zero;
		tendToPlaceMove = Vector3.zero;
		sphericalMove = Vector3.zero;

		foreach (Boid boid in boids) {
			List<Transform> nearbyBoidsAndObstacles = boid.GetNearbyObjects(boid); // Nearby boids and obstacles
			boid.boidMaterial.color = Color.Lerp(Color.white, Color.red, nearbyBoidsAndObstacles.Count / 6f); // 6 neighbors

			// Experiment with the weights (These work pretty smoothly at the moment so I hard coded them until I implement them in the UI)
			if (simUI.cohesionToggle.isOn) { cohesionMove = CohesionMove(boid, nearbyBoidsAndObstacles) * 1; }
			if (simUI.alignmentToggle.isOn) { alignmentMove = AlignmentMove(boid, nearbyBoidsAndObstacles) * 1; }
			if (simUI.separationToggle.isOn) { separationMove = SeparationMove(boid, nearbyBoidsAndObstacles) * 5; }
			if (simUI.steeredCohesionToggle.isOn) { steeredCohesionMove = SteeredCohesionMove(boid, nearbyBoidsAndObstacles) * 10; }
			if (simUI.sphericalMovementToggle.isOn) { sphericalMove = SphericalMovement(boid) * .3f; }

			// Check is "Lazy Flight" is turned on
			if (simUI.lazyFlightToggle.isOn) {
				tendToPlaceMove = TendToPlace(boid) * 1;
				Collider[] boidColliders = Physics.OverlapSphere(randomPosition, 8f);
				if (boidColliders.Length > boids.Count) { // I use '>' because Physics.OverlapSphere() is picking up the obstacles too
					randomPosition = GenerateRandomPoint();
				}
			}
			
			// Check if therei is a strong wind (button pressed from the UI)
			if (simUI.isStrongWind) {
				strongWind = StrongWind() * 1;
				ExecuteAfterTime(5f);
				simUI.isStrongWind = false;
			}

			stayBoundedMove = StayInBoundaries(boid, nearbyBoidsAndObstacles) * 1; // Don't really want to alter this from UI.

			// Add all movements vectors and then move each boid.
			// If I start adding more behaviors and tweaks, I will create a better way of implementing more of them. 
			boid.Move(cohesionMove + alignmentMove + separationMove + steeredCohesionMove + stayBoundedMove + strongWind + tendToPlaceMove + sphericalMove);
		}
	}

	private Vector3 CohesionMove(Boid boid, List<Transform> nearbyBoids) {
		// If no nearby boids, return zero vectro (aka don't change current movement)
		if (nearbyBoids.Count == 0) {
			return Vector3.zero;
		}

		// Add all of nearby points together
		Vector3 cohesionMove = Vector3.zero;
		foreach (Transform boidTransform in nearbyBoids) {
			cohesionMove += boidTransform.position;
		}

		cohesionMove /= nearbyBoids.Count; // Average all the nearby points
		cohesionMove -= boid.transform.position; // Offset the cohesionMove by boid's position
		return cohesionMove;
	}

	private Vector3 SteeredCohesionMove(Boid boid, List<Transform> nearbyBoids) {
		Vector3 currVelocity = Vector3.zero; // Temp placeholder for Vector3.SmoothDamp()

		// If no nearby boids, return zero vectro (aka don't change current movement)
		if (nearbyBoids.Count == 0) {
			return Vector3.zero;
		}

		// Add all of nearby points together
		Vector3 cohesionMove = Vector3.zero;
		foreach (Transform boidTransform in nearbyBoids) {
			cohesionMove += boidTransform.position;
		}

		cohesionMove /= nearbyBoids.Count; // Average all the nearby points
		cohesionMove -= boid.transform.position; // Offset the cohesionMove by boid's position
		cohesionMove = Vector3.SmoothDamp(boid.transform.forward, cohesionMove, ref currVelocity, boid.steerSmoothTime); // Smooth move out
		return cohesionMove;
	}

	private Vector3 AlignmentMove(Boid boid, List<Transform> nearbyBoids) {
		// If no nearby boids, maintain current alignment (direction)
		if (nearbyBoids.Count == 0) {
			return boid.transform.forward;
		}

		// Add all of nearby directions together
		Vector3 alignmentMove = Vector3.zero;
		foreach (Transform boidTransform in nearbyBoids) {
			alignmentMove += boidTransform.forward;
		}

		alignmentMove /= nearbyBoids.Count; // Average all the nearby directions
		return alignmentMove;
	}

	private Vector3 SeparationMove(Boid boid, List<Transform> nearbyBoidsAndObjects) {
		// If no nearby boids, don't change current movment
		if (nearbyBoidsAndObjects.Count == 0) {
			return Vector3.zero;
		}

		// Need to detect nearby boids or objects and calculate separation move
		Vector3 separationMove = Vector3.zero;
		int boidsToAvoid = 0;
		foreach (Transform transform in nearbyBoidsAndObjects) {
			// Check if nearby obstacles are present
			if (transform.gameObject.tag == "Obstacle") {

				Vector3 closestPoint = transform.gameObject.GetComponent<Collider>().ClosestPoint(boid.transform.position);
				if (Vector3.SqrMagnitude(closestPoint - boid.transform.position) < boid.obstacleAvoidanceRadius) {
					separationMove += boid.transform.position - closestPoint;

					// I am multiplying this by something (a weight) b/c I think I the obstacle avoidance should have a stronger effect
					// which is why I think I will create a separate function for this AKA "ObstacleMove()" down the road.
					return separationMove * 5; 
				}
			}

			// Check if nearby boids are present
			if (Vector3.SqrMagnitude(transform.position - boid.transform.position) < boid.squareAvoidanceRadius) {
				separationMove += boid.transform.position - transform.position;
				boidsToAvoid++;
			}
		}
		// Average out the move with nearby boids
		if (boidsToAvoid > 0) {
			separationMove /= boidsToAvoid;
		}
		return separationMove;
	}

	private Vector3 StayInBoundaries(Boid boid, List<Transform> nearbyBoids) {
		Vector3 centerOffset = globalCenter - boid.transform.position;
		float t = centerOffset.magnitude / globalRadius;

		if (t < .9f) {
			return Vector3.zero;
		}
		return centerOffset * t * t;
	}

	private Vector3 StrongWind() {
		// Returns random vector that pushes the entire flock a certain direction
		float x = Random.Range(-globalRadius, globalRadius);
		float y = Random.Range(-globalRadius, globalRadius);
		float z = Random.Range(-globalRadius, globalRadius);
		Vector3 strongWind = new Vector3(x, y, z);
		return strongWind;
	}

	private Vector3 TendToPlace(Boid boid) {
		// AKA "Lazy Flight"
		return (randomPosition - boid.transform.position);
	}

	private Vector3 GenerateRandomPoint() {
		float x = Random.Range(-globalRadius + 2, globalRadius-2);
		float y = Random.Range(-globalRadius + 2, globalRadius-2);
		float z = Random.Range(-globalRadius + 2, globalRadius-2);
		randomPosition = new Vector3(x, y, x);
		return randomPosition;
	}

	private Vector3 SphericalMovement(Boid boid) {
		// For now, this the movement will be centered around the origin (r = 1)
		// I placed a sphere directly at the origin to emphasize the movement
		float theta = Random.Range(0, 360) * Mathf.Deg2Rad;
		float phi = Random.Range(0, 360) * Mathf.Deg2Rad;

		float x = Mathf.Sin(theta) * Mathf.Cos(phi);
		float y = Mathf.Sin(theta) * Mathf.Sin(phi);
		float z = Mathf.Cos(theta);

		return (new Vector3(x, y, z) - boid.transform.position);
	}

	IEnumerator ExecuteAfterTime(float time) {
		yield return new WaitForSeconds(time);
	}

}