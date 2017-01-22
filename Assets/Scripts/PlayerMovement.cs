using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	public GameObject rotura1;
	public GameObject rotura2;
	public GameObject rotura3;
	public GameObject bat3;
	public GameObject bat2;
	public GameObject bat1;
	public GameObject bat0;
	public GameObject loadFull;
	public GameObject loadEmpty;

	public TextMesh scoreText;

	public GameObject hitFeedback;
	private SpriteRenderer hfRenderer;

	public GameObject markerDot;
	public float dotFrequency;

	public GameObject bullet;
	public float bulletSpeed = 1000f;

	//public IddleAnimation iddleAnimation;

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

	private AudioSource[] audios;

	private bool isGameOver = false;

	private float gameOverTimer = 0f;


	// Use this for initialization
	void Start () {
		pos = transform.position;
		axis = transform.up;
		frequency = startingFrequency;
		magnitude = startingMagnitude;


		audios = GetComponents<AudioSource>();

		hfRenderer = hitFeedback.GetComponent<SpriteRenderer> ();
		//iddleAnimation.iddlePlayer ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isGameOver) {
			gameOverTimer += 1f * Time.deltaTime;
			Debug.Log ("gameovertimer: " + gameOverTimer);
		}

		if (isGameOver && gameOverTimer > 5f) {
			gameOverTimer = 0f;
			isGameOver = false;
			SceneManager.LoadScene ("Menu");
		}

		if (isGameOver)
			return;


		//Input de disparo
		if (Input.GetKeyDown(KeyCode.Space) && canShoot) {
			Vector3 shootPoint = new Vector3 (transform.position.x + 1f, transform.position.y, transform.position.z);
			GameObject b = (GameObject)Instantiate (bullet, shootPoint, transform.rotation);
			Rigidbody2D rb = b.GetComponent<Rigidbody2D> ();
			rb.AddForce (b.transform.right * bulletSpeed);
			GetComponent<Animator> ().Play ("PlayerShoot");

			loadFull.SetActive (false);
			loadEmpty.SetActive (true);

			canShoot = false;
			audios [2].Play ();
		}

		//Input de + frecuencia
		if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && frequency < maxFrequency) {
			frequency += frequencyModValue;
		}

		//Input de - frecuencia
		if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.RightArrow)) && frequency > minFrequency) {
			frequency -= frequencyModValue;
		}
		//Tenemos que detectar cuando el jugador disminuye la magnitud,
		//pero no podemos cambiarlo automaticamente porque se ve feo
		//Si el usuario pidio disminuir la amplitud, almacenamos ese evento
		//y esperamos a estar dentro del rango de Y para realizar el cambio
			if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && plusMagnitude == false && magnitude < maxMagnitude) {
			plusMagnitude = true;
		}

			if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && minusMagnitude == false && magnitude > minMagnitude) {
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
		//Vector3 dotExit = new Vector3 (transform.position.x - 0.6f, transform.position.y, transform.position.z);

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
		if (other.tag == "HealthPickup")
			HealthPickupTouch (other);
	}

	void EnemyTouch(Collider2D other){
		audios[0].Play();

		life -= 1;
		switch (life) {
		case 2:
			rotura1.SetActive (true);
			bat3.SetActive (false);
			bat2.SetActive (true);
			break;
		case 1:
			rotura1.SetActive (false);
			rotura2.SetActive (true);
			bat2.SetActive (false);
			bat1.SetActive (true);
			break;
		case 0:
			rotura2.SetActive (false);
			rotura3.SetActive (true);
			bat1.SetActive (false);
			bat0.SetActive (true);
			scoreText.text = "GAME OVER\nScore: " + Mathf.Round (LevelManager.timer);
			scoreText.gameObject.SetActive (true);
			//gameObject.SetActive (false);
			SpriteRenderer sr = GetComponent<SpriteRenderer> ();
			sr.enabled = false;
			isGameOver = true;
			LevelManager.isGameOver = true;

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
	}

	void FadeOutHit(float progress) {
		Color color = hfRenderer.color;
		color.a = Mathf.Lerp (0f, 1f, progress);
		hfRenderer.color = color;
	}

	void PowerUpTouch(Collider2D other){
		loadEmpty.SetActive (false);
		loadFull.SetActive (true);
		canShoot = true;
		audios[1].Play();
		Destroy (other.gameObject);
	}

	void HealthPickupTouch(Collider2D other) {
		audios [1].Play ();
		Destroy (other.gameObject);

		if (life == 3)
			return;
		
		life += 1;
		switch (life) {
		case 3:
			rotura1.SetActive (false);
			bat2.SetActive (false);
			bat3.SetActive (true);
			break;
		case 2:
			rotura2.SetActive (false);
			rotura1.SetActive (true);
			bat1.SetActive (false);
			bat2.SetActive (true);
			break;
		default:
			break;
		}

	}
}
