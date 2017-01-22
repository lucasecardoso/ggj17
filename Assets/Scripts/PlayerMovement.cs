using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public int life;
	public float moveSpeed = 2.0f;

	public float startingFrequency = 5.0f;
	public float startingMagnitude = 5.0f;

	public float frequencyModValue = 1.0f;
	public float magnitudeModValue = 1.0f;

	public float maxFrequency = 8f;
	public float minFrequency = 2f;
	public float maxMagnitude = 8f;
	public float minMagnitude = 2f;

	public GameObject bat3;
	public GameObject bat2;
	public GameObject bat1;
	public GameObject bat0;

	public GameObject hitFeedback;
	private SpriteRenderer hfRenderer;

	public GameObject markerDot;
	public float dotFrequency;

	public GameObject bullet;
	public float bulletSpeed = 1000f;

	private float dotCounter;

	//public Text text;

	private Vector2 axis;
	private Vector2 pos;

	private float frequency;
	private float magnitude;
	private float phase = 0f;

	private bool plusMagnitude;
	private bool minusMagnitude;

	private bool canShoot = true;

	private bool shouldFadeIn = false;
	private bool shouldFadeOut = false;
	private float fadeInProgress = 0f;

	// Use this for initialization
	void Start () {
		pos = transform.position;
		axis = transform.up;
		frequency = startingFrequency;
		magnitude = startingMagnitude;

		hfRenderer = hitFeedback.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Input de disparo
		if (Input.GetButtonDown("Fire1") && canShoot) {
			GameObject b = (GameObject)Instantiate (bullet, transform.position, transform.rotation);
			Rigidbody2D rb = b.GetComponent<Rigidbody2D> ();
			rb.AddForce(b.transform.right * bulletSpeed);
			canShoot = false;
		}

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

		checkFadeInOut ();
	}
		
	void checkFadeInOut() {
		if (!shouldFadeIn && !shouldFadeOut)
			return;
		
		if (shouldFadeIn && fadeInProgress < 1f) {
			fadeInProgress += 4f * Time.deltaTime;
			FadeInHit (fadeInProgress);
		}

		if (fadeInProgress >= 1f) {
			shouldFadeIn = false;
			shouldFadeOut = true;
		}

		if (shouldFadeOut && (fadeInProgress - 4f * Time.deltaTime) > 0f) {
			fadeInProgress -= 4f * Time.deltaTime;
			FadeOutHit (fadeInProgress);
		} else if (shouldFadeOut) {
			fadeInProgress = 0f;
			FadeOutHit (fadeInProgress);
		}

		if (fadeInProgress <= 0f) {
			shouldFadeOut = false;
			shouldFadeIn = false;

			//hitFeedback.SetActive (false);
		}

		Debug.Log ("fadeinprogress = " + fadeInProgress);
	}

	//Funcion que calcula la nueva fase tras el cambio de frecuencia
	void CalculateNewFrequency() {
		float curr = (Time.time * startingFrequency + phase) % (2.0f * Mathf.PI);
		float next = (Time.time * frequency) % (2.0f * Mathf.PI);
		phase = curr - next;
		startingFrequency = frequency;
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Enemy") {
			EnemyTouch (other);
			shouldFadeIn = true;
		}
		if (other.tag == "PowerUp")
			PowerUpTouch (other);
	}

	void EnemyTouch(Collider2D other){
		life -= 1;
		switch (life) {
		case 2:
			bat3.SetActive (false);
			bat2.SetActive (true);
			break;
		case 1:
			bat2.SetActive (false);
			bat1.SetActive (true);
			break;
		case 0:
			bat1.SetActive (false);
			bat0.SetActive (true);
			Time.timeScale = 0;
			break;
		}

		Color c = hfRenderer.color;
		c.a = 0f;

		hfRenderer.color = c;

		hitFeedback.SetActive (true);

		Destroy(other.gameObject);
	}

	void FadeInHit(float progress) {
		Color color = hfRenderer.color;
		color.a = Mathf.Lerp (0f, 1f, progress);
		hfRenderer.color = color;

		Debug.Log ("fadeinhit");
	}

	void FadeOutHit(float progress) {
		Color color = hfRenderer.color;
		color.a = Mathf.Lerp (0f, 1f, progress);
		hfRenderer.color = color;

		Debug.Log ("fadeouthit");
	}

	void PowerUpTouch(Collider2D other){
		Debug.Log ("PowerUpTouch");

		Destroy (other.gameObject);
	}
}
