using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IddleAnimation : MonoBehaviour {

	public void iddlePlayer(){
		GetComponent<Animation> ().Play ();
		Debug.Log ("Animation iddle");
	}
}
