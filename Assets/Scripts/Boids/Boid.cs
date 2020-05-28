using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {


    public float maxSpeed;
    public Collider collider; // Cached

    public Material boidMaterial; // Cached

    [HideInInspector]
    public float steerSmoothTime = 10f;

    public Flock flock;

    private void Awake() {
        collider = GetComponent<SphereCollider>();
        boidMaterial = GetComponent<Renderer>().material;
    }

    public void Move(Vector3 velocity) {
        // Clamp speed
        if (velocity.sqrMagnitude > maxSpeed) {
            velocity = velocity.normalized * maxSpeed;
        }

        transform.forward = velocity; // Changes the direction
        transform.position += velocity * Time.deltaTime; // Time.delta smoothes out the position change
    }


}
