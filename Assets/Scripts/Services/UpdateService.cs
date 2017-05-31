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
		foreach (UpdateType en in Enum.GetValues (typeof (UpdateType))) {
			subscribers.Add (en, new List<Action<String, Dictionary<String, String>>> ());	
		}
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
			Debug.LogWarning ("Send update of type " + GetData<UpdateType> (message, "type") + " from " + CurrentUser.GetInstance ().GetUserInfo ().username);
			ChatService.GetInstance ().SendPrivateMessage (target, message);
		}
	}

	public IEnumerator Wait (float seconds) {
		yield return new WaitForSeconds (seconds);
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

	public static T GetData<T> (Dictionary<String, String> message, string key) {
		return JsonUtility.FromJson<T>(message[key]);
	}

	public static KeyValuePair<string, string> CreateKV<T> (string key, T value) {
		return new KeyValuePair<string, string> (key, JsonUtility.ToJson (value));
	}
}
