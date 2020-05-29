using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    public Flock flock;
    public Collider collider; // Cached
    public Material material; // Cached
    public float maxSpeed;

    

    private void Awake() {
        collider = GetComponent<SphereCollider>();
        material = GetComponent<Renderer>().material;
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
