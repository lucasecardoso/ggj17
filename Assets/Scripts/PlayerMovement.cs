using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float moveSpeed = 2.0f;

	public float startingFrequency = 5.0f;
	public float startingMagnitude = 5.0f;

	public float frequencyModValue = 1.0f;
	public float magnitudeModValue = 1.0f;

	public GameObject markerDot;
	public float dotFrequency;

	private float dotCounter;

	public Text text;

	private Vector2 axis;
	private Vector2 pos;

	private float frequency;
	private float magnitude;
	private float phase = 0f;

	private bool plusMagnitude;
	private bool minusMagnitude;


	// Use this for initialization
	void Start () {
		pos = transform.position;
		axis = transform.up;
		frequency = startingFrequency;
		magnitude = startingMagnitude;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyUp(KeyCode.W)) {
			frequency += frequencyModValue;
		}

		if (Input.GetKeyUp(KeyCode.S)) {
			frequency -= frequencyModValue;
		}

		if (Input.GetKeyUp(KeyCode.UpArrow) && plusMagnitude == false) {
			plusMagnitude = true;
		}

		if (Input.GetKeyUp(KeyCode.DownArrow) && minusMagnitude == false) {
			minusMagnitude = true;
		}

		if (frequency != startingFrequency) {
			CalculateNewFrequency();
		}

		if (plusMagnitude && transform.position.y < 0.5 && transform.position.y > -0.5) {
			magnitude += magnitudeModValue;
			plusMagnitude = false;
		}

		if (minusMagnitude && transform.position.y < 0.5 && transform.position.y > -0.5) {
			magnitude -= magnitudeModValue;
			minusMagnitude = false;
		}

		pos += (Vector2)transform.right * Time.deltaTime * moveSpeed;

		Vector2 v2 = (Vector2)pos;
		v2.y = Mathf.Sin(Time.time * startingFrequency + phase) * magnitude;
		transform.position = v2;

		text.text = "Position: " + transform.position.x + ", " + transform.position.y;

		dotCounter += 1f * Time.deltaTime;

		if (dotCounter > dotFrequency) {
			Instantiate (markerDot, transform.position, transform.rotation);
		}

	}

	void CalculateNewFrequency() {
		float curr = (Time.time * startingFrequency + phase) % (2.0f * Mathf.PI);
		float next = (Time.time * frequency) % (2.0f * Mathf.PI);
		phase = curr - next;
		startingFrequency = frequency;
	}

	void CalculateNewMagnitude() {
		
	}
}
