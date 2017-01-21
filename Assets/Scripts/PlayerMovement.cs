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

	public float maxFrequency = 8f;
	public float minFrequency = 2f;
	public float maxMagnitude = 8f;
	public float minMagnitude = 2f;

	public GameObject markerDot;
	public float dotFrequency;

	private float dotCounter;

	//public Text text;

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

		//Input de + frecuencia
		if (Input.GetKeyUp(KeyCode.W) && frequency < maxFrequency) {
			frequency += frequencyModValue;
		}

		//Input de - frecuencia
		if (Input.GetKeyUp(KeyCode.S) && frequency > minFrequency) {
			frequency -= frequencyModValue;
		}

		//Tenemos que detectar cuando el jugador disminuye la magnitud,
		//pero no podemos cambiarlo automaticamente porque se ve feo
		//Si el usuario pidio disminuir la amplitud, almacenamos ese evento
		//y esperamos a estar dentro del rango de Y para realizar el cambio
		if (Input.GetKeyUp(KeyCode.UpArrow) && plusMagnitude == false && magnitude < maxMagnitude) {
			plusMagnitude = true;
		}

		if (Input.GetKeyUp(KeyCode.DownArrow) && minusMagnitude == false && magnitude > minMagnitude) {
			minusMagnitude = true;
		}

		//Si pedimos una nueva frecuencia, tenemos que calcular la fase nueva
		if (frequency != startingFrequency) {
			CalculateNewFrequency();
		}

		if (plusMagnitude && transform.position.y < 0.2 && transform.position.y > -0.2) {
			magnitude += magnitudeModValue;
			plusMagnitude = false;
		}

		if (minusMagnitude && transform.position.y < 0.2 && transform.position.y > -0.2) {
			magnitude -= magnitudeModValue;
			minusMagnitude = false;
		}

		pos += (Vector2)transform.right * Time.deltaTime * moveSpeed;

		Vector2 v2 = (Vector2)pos;
		v2.y = Mathf.Sin(Time.time * startingFrequency + phase) * magnitude;
		transform.position = v2;

		//text.text = "Position: " + transform.position.x + ", " + transform.position.y;

		Instantiate (markerDot, transform.position, transform.rotation);
		//dotCounter = 0f;

	}

	//Funcion que calcula la nueva fase tras el cambio de frecuencia
	void CalculateNewFrequency() {
		float curr = (Time.time * startingFrequency + phase) % (2.0f * Mathf.PI);
		float next = (Time.time * frequency) % (2.0f * Mathf.PI);
		phase = curr - next;
		startingFrequency = frequency;
	}

}
