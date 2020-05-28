using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockGenerator : MonoBehaviour {

    public Flock flockPrefab;
    public int flockSpawnCount;

    
    public List<Flock> flocks = new List<Flock>();

	public void CreateFlock(Flock flock, int numBoids) {

        float xCord = Random.Range(-50, 50);
        float yCord = Random.Range(-50, 50);
        float zCord = Random.Range(-50, 50);

        Vector3 position = new Vector3(xCord, yCord, zCord);

        Flock newFlock = Instantiate(flock);
        newFlock.gameObject.name = "Flock";
        newFlock.transform.localPosition = position;
        newFlock.CreateBoids(numBoids);

        //List<Boid> boids = newFlock.CreateBoids(numBoids);
        flocks.Add(newFlock);
        Debug.Log(flocks.Count);

    }

    public void ClearFlocks() {
        foreach (Flock flock in flocks) {

            if (flock != null) {
                if (UnityEditor.EditorApplication.isPlaying) {
                    Destroy(flock.gameObject);
                }
                else {
                    DestroyImmediate(flock.gameObject);
				}
                flock.ClearBoids();

			}
        }
        flocks.Clear();
    }



}