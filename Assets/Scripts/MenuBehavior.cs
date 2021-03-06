﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehavior : MonoBehaviour {

	int currentPos = 0;

	Vector3 initialPosition;

	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
		LevelManager.isGameOver = false;
		LevelManager.timer = 0f;
		LevelManager.bossIsHere = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.DownArrow) && currentPos < 3) {
			currentPos++;
		}

		if (Input.GetKeyDown (KeyCode.UpArrow) && currentPos > 0) {
			currentPos--;
		}

		transform.position = new Vector2 (initialPosition.x, initialPosition.y - (60f * currentPos));

		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.Return)) {
			switch (currentPos) {
			case 0:
				SceneManager.LoadScene ("Level1");
				break;
			case 1:
				SceneManager.LoadScene ("Tutorial");
				break;
			case 2:
				SceneManager.LoadScene ("Credits");
				break;
			case 3:
				Application.Quit ();
				break;
			}
		}
	}
}
