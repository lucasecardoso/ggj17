using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject player;
	//public GameObject spawnPoint1;
	public GameObject[] enemies;
	public GameObject shooterEnemy;
	public GameObject[] powerUps;
	public GameObject[] spawnPoints;
	public GameObject boss;
	int lastRandomIndex = 5;

	public float spawnTimer = 2f;

	public float bossTimer = 20f;

	private float counter = 0f;

	private float audioCounter = 0f;

	public static bool bossIsHere = false;

	private float bossCounter = 0f;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		counter += 1f * Time.deltaTime;

		//Contamos el tiempo del boss si el boss no esta
		if (!bossIsHere)
			bossCounter += 1f * Time.deltaTime;

		audioCounter += 1f * Time.deltaTime;

		//Instanciamos el boss en caso de que el timer pase del limite
		if (bossCounter > bossTimer) {
			bossIsHere = true;
			GameObject b = (GameObject)Instantiate(boss, spawnPoints[1].transform.position, spawnPoints[1].transform.rotation);

			//Lo asignamos como hijo del spawnPoint asi se mueve en relacion a la pantalla
			b.transform.parent = spawnPoints[1].transform;
			bossCounter = 0f;
		}

		if (counter > spawnTimer) {
			
			int index = Random.Range (0, 3);

			while (index == lastRandomIndex) {
				index = Random.Range (0, 3);
			}

			lastRandomIndex = index;

			int spawn = Random.Range (0, 10);
			switch(spawn){
			case 1:
				Instantiate (powerUps[0], spawnPoints [index].transform.position, spawnPoints [index].transform.rotation);
				break;
			case 2:
				Instantiate (powerUps [1], spawnPoints [index].transform.position, spawnPoints [index].transform.rotation);
				break;
			default:
				//Si el boss esta, solamente instanciamos powerups
				if (bossIsHere) {
					break;
				}

				int spawneableEnemy = Random.Range (0, enemies.Length);
				if (index == 2) {
					if ((spawneableEnemy % 2) != 1) {
						Instantiate (shooterEnemy, spawnPoints [index].transform.position, spawnPoints [index].transform.rotation);
					} else {
						Instantiate (enemies [spawneableEnemy], spawnPoints [index].transform.position, spawnPoints [index].transform.rotation);
					}
				} else {
					Instantiate (enemies[spawneableEnemy], spawnPoints [index].transform.position, spawnPoints [index].transform.rotation);
				}
				break;
			}
			counter = 0f;
		}

		if (audioCounter > 1f) {
			audioSource.Play();
			audioCounter = 0f;
		}
	}
}

