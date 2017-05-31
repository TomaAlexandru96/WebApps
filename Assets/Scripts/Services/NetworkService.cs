using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkService : Photon.MonoBehaviour {

	public const String GAME_VERSION = "v0.01";
	public const String partyPrefabName = "Party";
	public Text infoLabel;
	private static NetworkService instance = null;

	public void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	public void StartService () {
		SetupConnection ();
	}

	public void StopService () {
		DestroyConnection ();
	}

	public static NetworkService GetInstance () {
		return instance;
	}

	public void SetupConnection () {
		PhotonNetwork.ConnectUsingSettings (GAME_VERSION);
	}

	public void OnJoinedLobby () {
		RoomOptions options = new RoomOptions ();
		PhotonNetwork.JoinOrCreateRoom (ChatService.GetInstance ().GetPartyCHName (), options, TypedLobby.Default);
	}

	private void DestroyConnection () {
		PhotonNetwork.Disconnect ();
	}

	public void OnGui () {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	public void JoinRoom (string roomName) {
		PhotonNetwork.JoinRoom (roomName);
	}
}
