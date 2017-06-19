using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectMaps : MonoBehaviour {

	public GameObject thisScene;
	public GameObject nextScene;
	public string newRoomName;
	public Transform spawn;

	void OnTriggerEnter2D(Collider2D coll) {
		thisScene.SetActive (false);
		nextScene.SetActive (true);
		GameObject.FindGameObjectWithTag ("Player").transform.position = spawn.position;
		GameObject.FindGameObjectWithTag ("RoomName").GetComponent<RoomName> ().UpdateRoomName (newRoomName);
	}
}
