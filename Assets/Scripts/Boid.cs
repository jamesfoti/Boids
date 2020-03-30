using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
    [HideInInspector] 
    public float neighborRadius = 2f;
    private float avoidanceRadiusMultiplier = .5f;
    private float squareNeighborRadius;

    [HideInInspector] 
    public float squareAvoidanceRadius;

    [HideInInspector] 
    public float obstacleAvoidanceRadius = 35f;

    private float maxSpeed = 4;

    private Collider collider; // Cached
    public Material boidMaterial; // Cached

    [HideInInspector]
    public float steerSmoothTime = 10f;

    private void Awake() {
        collider = GetComponent<SphereCollider>();
        boidMaterial = GetComponent<Renderer>().material;
    }

    private void Start() {
        // Cached so Unity doesn't have to square(num) so many times in order to increase efficiency
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier; 
    }

    public void Move(Vector3 velocity) {
        // Clamp speed
        if (velocity.sqrMagnitude > maxSpeed) {
            velocity = velocity.normalized * maxSpeed;
        }

        transform.forward = velocity; // Changes the direction
        transform.position += velocity * Time.deltaTime; // Time.delta smoothes out the position change
    }

    public List<Transform> GetNearbyObjects(Boid boid) {
        List<Transform> boidTransforms = new List<Transform>();
        Collider[] boidColliders = Physics.OverlapSphere(boid.transform.position, neighborRadius);

        foreach (Collider c in boidColliders) {
            if (c != boid) {
                boidTransforms.Add(c.transform);
            }
        }
        return boidTransforms;
    }
}
