using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon.Chat;

public class UpdateService : MonoBehaviour {
	
	private static UpdateService instance = null;
	private Dictionary<UpdateType, List<Action<String, Dictionary<String, String>>>> subscribers = new Dictionary<UpdateType, List<Action<String, Dictionary<String, String>>>> ();

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
		subscribers.Add (UpdateType.PartyRequest, new List<Action<String, Dictionary<String, String>>> ());
		subscribers.Add (UpdateType.UserUpdate, new List<Action<String, Dictionary<String, String>>> ());
		subscribers.Add (UpdateType.PartyRequestAccept, new List<Action<String, Dictionary<String, String>>> ());
	}

	public static UpdateService GetInstance () {
		return instance;
	}

	public static Dictionary<String, String> CreateMessage (UpdateType type, params KeyValuePair<String, String>[] els) {
		Dictionary<String, String> message = new Dictionary<String, String> ();

		message.Add ("type", JsonUtility.ToJson (type));
		foreach (var pair in els) {
			message.Add (pair.Key, pair.Value);
		}

		return message;
	}

	public void SendUpdate (string[] targets, Dictionary<String, String> message) {
		foreach (var target in targets) {
			ChatService.GetInstance ().SendPrivateMessage (target, message);
		}
	}

	public void Recieve (string sender, Dictionary<String, String> message) {
		List<Action<String, Dictionary<String, String>>> functions;
		subscribers.TryGetValue (JsonUtility.FromJson<UpdateType> (message["type"]), out functions);

		foreach (var func in functions) {
			func (sender, message);
		}
	}

	public void Subscribe (UpdateType ev, Action<String, Dictionary<String, String>> func) {
		List<Action<String, Dictionary<String, String>>> functions;
		subscribers.TryGetValue (ev, out functions);
		functions.Add (func);
	}
}
