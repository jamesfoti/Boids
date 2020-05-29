using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class FlockGenerator : MonoBehaviour {

    private Flock flock;

    [Range(0, 100)]
    [Tooltip("Used to spawn a certain number of flocks at Start().")]
    public int flockSpawnCount;

    [HideInInspector]
    public List<Flock> flocks = new List<Flock>();


	private void Awake() {
        this.flock = GetComponent<Flock>();
	}

	private void Start() {
		for (int i = 0; i < flockSpawnCount; i++) {
            CreateFlock();
		}
	}

	public void CreateFlock() {
        float xCord = Random.Range(-50, 50);
        float yCord = Random.Range(-50, 50);
        float zCord = Random.Range(-50, 50);

        Vector3 randomPosition = new Vector3(xCord, yCord, zCord);

        Flock newFlock = Instantiate(this.flock);
        newFlock.gameObject.name = "Flock";
        newFlock.transform.localPosition = randomPosition;

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