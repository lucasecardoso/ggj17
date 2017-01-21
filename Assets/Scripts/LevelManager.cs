using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject player;
	public GameObject spawnPoint1;
	public GameObject enemy;

	public float spawnTimer = 2f;

	private float counter = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		counter += 1f * Time.deltaTime;

		if (counter > spawnTimer) {
			Instantiate (enemy, spawnPoint1.transform.position, spawnPoint1.transform.rotation);
			counter = 0f;
		}
	}
}

