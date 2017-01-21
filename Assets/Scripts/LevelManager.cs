using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject player;
	//public GameObject spawnPoint1;
	public GameObject enemy;
	public GameObject[] spawnPoints;

	public float spawnTimer = 2f;

	private float counter = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		counter += 1f * Time.deltaTime;

		if (counter > spawnTimer) {
			int index = Random.Range (0, 3);
			Instantiate (enemy, spawnPoints[index].transform.position, spawnPoints[index].transform.rotation);
			counter = 0f;
		}
	}
}

