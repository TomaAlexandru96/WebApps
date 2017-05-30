using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon.Chat;

public class UpdateService : MonoBehaviour {
	
	private static UpdateService instance = null;
	private Dictionary<UpdateType, List<Action<String>>> subscribers = new Dictionary<UpdateType, List<Action<String>>> ();

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
		subscribers.Add (UpdateType.PartyRequest, new List<Action<String>> ());
		subscribers.Add (UpdateType.UserUpdate, new List<Action<String>> ());
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
		List<Action<String>> functions;
		subscribers.TryGetValue (messageType, out functions);

		foreach (var func in functions) {
			func (sender);
		}
	}

	public void Subscribe (UpdateType ev, Action<String> func) {
		List<Action<String>> functions;
		subscribers.TryGetValue (ev, out functions);
		functions.Add (func);
	}
}
