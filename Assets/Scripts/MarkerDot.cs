using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerDot : MonoBehaviour {

	public float timeToLive = 2f;

	private float counter = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		counter += 1f * Time.deltaTime;

		if (counter > timeToLive)
			Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "DeathZone")
			Destroy (gameObject);
	}
}
