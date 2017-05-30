using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon.Chat;

public class UpdateService : MonoBehaviour {
	
	private static UpdateService instance = null;
	public const string CH_NAME = "Secret";

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

	public void SendUpdate (string[] to, UpdateType message) {
		foreach (var target in to) {
			ChatService.GetInstance ().SendPrivateMessage (target, message);
		}
	}

	public void Recieve (string sender, UpdateType messageType) {
		switch (messageType) {
		case UpdateType.UserUpdate: CurrentUser.GetInstance ().RequestUpdate (); break;
		case UpdateType.PartyRequest: Debug.Log ("Request for party from: " + sender); break;
		}
	}

	public void Setup () {
		ChatService.GetInstance ().Subscribe (new string[]{CH_NAME});
	}

	public void Teardown () {
		ChatService.GetInstance ().Unsubscribe (new string[]{CH_NAME});
	}
}
