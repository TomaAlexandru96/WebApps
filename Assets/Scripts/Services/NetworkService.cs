using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkService : MonoBehaviour {

	public const String GAME_VERSION = "v0.01";
	private static NetworkService instance = null;

	void Awake () {
		if (instance == null) {
			instance = this;
			PhotonNetwork.ConnectUsingSettings(GAME_VERSION);
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	public static NetworkService GetInstance () {
		return instance;
	}

	void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
}
