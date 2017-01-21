using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterEnemyBehavior : MonoBehaviour {

	public float bulletThrust = 1000f;

	public GameObject bullet;

	private float timer = 0f;
	private bool canShoot = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		timer += 1f * Time.deltaTime;

		if (timer > 0.8f && canShoot) {
			Shoot ();
			canShoot = false;
		}
	}

	private void Shoot () {
		Vector3 spawn = new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z);
		GameObject b = (GameObject)Instantiate (bullet, spawn, transform.rotation);
		b.GetComponent<Rigidbody2D> ().AddForce (b.transform.up * bulletThrust);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "DeathZone" || other.tag == "Enemy")
			Destroy (gameObject);
	}
}
