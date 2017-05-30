using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon.Chat;

public class UpdateService : MonoBehaviour {
	
	private static UpdateService instance = null;
	private Dictionary<UpdateType, List<Action>> subscribers = new Dictionary<UpdateType, List<Action>> ();

	public void Awake () {
		if (instance == null) {
			instance = this;
			InstantiateSubs ();
			DontDestroyOnLoad (gameObject);
		} else {
			DestroyImmediate (gameObject);
		}
	}

	private void InstantiateSubs () {
		subscribers.Add (UpdateType.PartyRequest, new List<Action> ());
		subscribers.Add (UpdateType.UserUpdate, new List<Action> ());
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
		List<Action> functions;
		subscribers.TryGetValue (messageType, out functions);

		foreach (var func in functions) {
			func ();
		}
	}

	public void Subscribe (UpdateType ev, Action func) {
		List<Action> functions;
		subscribers.TryGetValue (ev, out functions);
		functions.Add (func);
	}
}
