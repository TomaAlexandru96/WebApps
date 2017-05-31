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
			SetupConnection ();
			DontDestroyOnLoad (gameObject);
		} else {
			DestroyImmediate (gameObject);
		}
	}

	public static NetworkService GetInstance () {
		return instance;
	}

	public void SetupConnection () {
		PhotonNetwork.ConnectUsingSettings (GAME_VERSION);
		// set current user events
		UpdateService.GetInstance ().Subscribe (UpdateType.UserUpdate, (sender, message) => {
			CurrentUser.GetInstance ().RequestUpdate ();
		});
		UpdateService.GetInstance ().SendUpdate (CurrentUser.GetInstance ().GetUserInfo ().friends, 
			UpdateService.CreateMessage (UpdateType.UserUpdate));
	}

	public void OnJoinedLobby () {
		RoomOptions options = new RoomOptions ();
		PhotonNetwork.JoinOrCreateRoom (ChatService.GetInstance ().GetPartyCHName (), options, TypedLobby.Default);
	}

	public void DestroyConnection () {
		PhotonNetwork.Disconnect ();
		UpdateService.GetInstance ().SendUpdate (CurrentUser.GetInstance ().GetUserInfo ().friends, 
			UpdateService.CreateMessage (UpdateType.UserUpdate));
	}

	public void Update () {
		infoLabel.text = PhotonNetwork.connectionStateDetailed.ToString();
	}

	public void JoinRoom (string roomName) {
		PhotonNetwork.JoinRoom (roomName);
	}
}
