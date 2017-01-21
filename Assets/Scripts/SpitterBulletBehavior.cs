using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterBulletBehavior : MonoBehaviour {

	Rigidbody2D rb;
	private float timeToLive = 5f;
	private float counter;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (rb.velocity.y < 0f) {
			//Aca hay que animar para que tenga el inverso de la bala o lo que fuere
		}

		counter += 1f * Time.deltaTime;

		if (counter > timeToLive) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "ShooterEnemy")
			Destroy (gameObject);
	}
}
