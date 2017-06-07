using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectMaps : MonoBehaviour {

	public GameObject thisScene;
	public GameObject nextScene;

	public void Start () {
		thisScene.SetActive (true);
		nextScene.SetActive (false);

	}

	void OnTriggerEnter2D(Collider2D coll) {

		thisScene.SetActive (false);
		nextScene.SetActive (true);

	}
		


}
