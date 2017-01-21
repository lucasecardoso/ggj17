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
		GameObject b = (GameObject)Instantiate (bullet, transform.position, transform.rotation);
		b.GetComponent<Rigidbody2D> ().AddForce (b.transform.up * bulletThrust);
	}
}
