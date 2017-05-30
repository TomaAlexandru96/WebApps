using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon.Chat;

public class UpdateService : MonoBehaviour {
	
	private static UpdateService instance = null;

	public void Awake () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			DestroyImmediate (gameObject);
		}
	}

	public static UpdateService GetInstance () {
		return instance;
	}

	public void SendUpdate (string[] targets, UpdateType message) {
		foreach (var target in targets) {
			ChatService.GetInstance ().SendPrivateMessage (target, message);
		}
	}

	public void Recieve (string sender, UpdateType messageType) {
		switch (messageType) {
		case UpdateType.UserUpdate: CurrentUser.GetInstance ().RequestUpdate (); break;
		case UpdateType.PartyRequest: Debug.Log ("Request for party from: " + sender); break;
		}
	}
}
