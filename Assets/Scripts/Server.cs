using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server {

	// Use this for initialization
	void Start () {
		NetworkServer.Listen (1073);
	}
	
	// Update is called once per frame
	void Update () {
		if (NetworkServer.active) {
			Debug.Log ("Connected");
		}
	}
}
