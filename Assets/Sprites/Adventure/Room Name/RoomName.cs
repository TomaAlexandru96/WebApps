using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomName : MonoBehaviour {

	public GameObject roomName; 

	// Use this for initialization
	void Start () {
		Destroy (roomName, 5.0f);
	}
}
