using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject player;
	//public GameObject spawnPoint1;
	public GameObject enemy;
	public GameObject shooterEnemy;
	public GameObject powerUp;
	public GameObject[] spawnPoints;
	int lastRandomIndex = 5;

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

			while (index == lastRandomIndex) {
				index = Random.Range (0, 3);
			}

			lastRandomIndex = index;

			int spawn = Random.Range (0, 10);
			switch(spawn){
			case 1:
				Instantiate (powerUp, spawnPoints [index].transform.position, spawnPoints [index].transform.rotation);
				break;
			default:
				if (index == 2) {
					Instantiate (shooterEnemy, spawnPoints [index].transform.position, spawnPoints [index].transform.rotation);
				} else {
					Instantiate (enemy, spawnPoints [index].transform.position, spawnPoints [index].transform.rotation);
				}
				break;
			}
			counter = 0f;
		}
	}
}

