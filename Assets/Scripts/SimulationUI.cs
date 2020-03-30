using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationUI : MonoBehaviour {
    public Toggle cohesionToggle;
    public Toggle steeredCohesionToggle;
    public Toggle alignmentToggle;
    public Toggle separationToggle;
    public Toggle lazyFlightToggle;
    public Toggle sphericalMovementToggle;

    public Button strongWindButton;
    public GameObject pausePanel;
    public Slider flockSizeSlider;

    public bool isStrongWind = false;
    public bool isMoving = false;
    public Flock flock;
    
    private void Awake() {
        // I had to do these for the toggles b/c for some reason, dragging their references over in the editor doesn't work
        cohesionToggle = GameObject.Find("CohesionToggle").GetComponent<Toggle>();
        steeredCohesionToggle = GameObject.Find("SteeredCohesionToggle").GetComponent<Toggle>();
        alignmentToggle = GameObject.Find("AlignmentToggle").GetComponent<Toggle>();
        separationToggle = GameObject.Find("SeparationToggle").GetComponent<Toggle>();
        lazyFlightToggle = GameObject.Find("LazyFlightToggle").GetComponent<Toggle>();
        sphericalMovementToggle = GameObject.Find("SphericalMovementToggle").GetComponent<Toggle>();
    }

    private void Start() {
        flockSizeSlider.value = flock.defaultFlockSize;
    }

    public void StrongWindButton() {
        isStrongWind = true;
    }

    public void GenerateFlock() {
        flock.InitiatePositions();
    }

    public void StartSimulation() {
        isMoving = true;
    }

    public void AddBoid() {
        flock.CreateBoid();
    }

    public void Pause() {
        Debug.Log("Pause");
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void Resume() {
        Debug.Log("Resume");
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void ResetProgram() {
        Debug.Log("Reset Program");

        Time.timeScale = 1f;
        pausePanel.SetActive(false);

        cohesionToggle.isOn = true;
        steeredCohesionToggle.isOn = true;
        alignmentToggle.isOn = true;
        separationToggle.isOn = true;
        lazyFlightToggle.isOn = false;
        sphericalMovementToggle.isOn = false;

        isStrongWind = false;
        isMoving = false;

        flockSizeSlider.value = flock.defaultFlockSize;
        GenerateFlock();
    }

    public void ExitProgram() {
        Debug.Log("Eixt Program");
        Application.Quit();
    }
}