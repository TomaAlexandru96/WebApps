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
	private static TypedLobby adventureLobby = new TypedLobby ("Adventure", LobbyType.Default);
	private static TypedLobby endlessLobby = new TypedLobby ("Endless", LobbyType.Default);

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

	private void DestroyConnection () {
		PhotonNetwork.Disconnect ();
	}

	public void OnGui () {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	public RoomInfo[] GetAdventureRooms () {
		return PhotonNetwork.GetRoomList ();
	}

	public void JoinAdventureLobby () {
		PhotonNetwork.JoinLobby (adventureLobby);
	}

	public void JoinEndlessLobby () {
		PhotonNetwork.JoinLobby (endlessLobby);
	}

	public void JoinRoom (string roomName) {
		PhotonNetwork.JoinRoom (roomName);
	}

	public void CreateRoom (string roomName) {
		PhotonNetwork.CreateRoom (roomName, new RoomOptions () {MaxPlayers = 4}, PhotonNetwork.lobby);
	}

	public void LeaveRoom () {
		PhotonNetwork.LeaveRoom ();
	}
}
