using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network : MonoBehaviour {
	public void Start() {
		Debug.Log("Start");
		PhotonNetwork.ConnectUsingSettings("v0.01");
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
}
