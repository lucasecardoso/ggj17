using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour {

	private float initialMoveProgress = 0f;

	private Vector3 initialPosition;

	public int life = 3;

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
	}

	void Shoot() {
		Instantiate(projectile, transform.position, transform.rotation);
		canShoot = false;
		reloadCounter = 0f;
	}
}
