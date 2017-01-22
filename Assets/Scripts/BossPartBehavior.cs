using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPartBehavior : MonoBehaviour {

	public BossBehavior bossBehavior;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("entered trigger");
		if (other.tag == "Bullet") {
			bossBehavior.DamageBoss ();
			Destroy (other.gameObject);
		}
	}
}
