﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour {

	private float initialMoveProgress = 0f;

	private Vector3 initialPosition;

	public int life = 1;

	public GameObject projectile;

	private bool canShoot = false;

	public float shootReloadTime = 3f;

	private float reloadCounter = 0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (initialMoveProgress < 1f) {
			initialMoveProgress += 1f * Time.deltaTime;
			transform.localPosition = Vector3.Lerp(initialPosition, new Vector3(initialPosition.x - 2f, initialPosition.y, initialPosition.z), initialMoveProgress);;
		}

		if (canShoot)
			Shoot();
		
		reloadCounter += 1f * Time.deltaTime;

		if (reloadCounter > shootReloadTime) 
			canShoot = true;


		if (life <= 0) {
			Destroy(gameObject);
			LevelManager.bossIsHere = false;
		}
	}

	public void DamageBoss() {
		life -= 1;

	}

	void Shoot() {
		int yPosition = 0;

		switch (life) {
		case 3:
			yPosition = 0;
			break;
		case 2:
			yPosition = 3;
			break;
		case 1:
			yPosition = -3;
			break;
		default:
			yPosition = 0;
			break;
		}

		Instantiate(projectile, new Vector2(transform.position.x, yPosition), transform.rotation);
		canShoot = false;
		reloadCounter = 0f;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Bullet") {
			DamageBoss ();
			Destroy (other.gameObject);
		}
	}

}
