using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon.Chat;

public class UpdateService : MonoBehaviour {
	
	private static UpdateService instance = null;
	private Dictionary<UpdateType, List<Action<String, Dictionary<String, String>>>> subscribers = new Dictionary<UpdateType, List<Action<String, Dictionary<String, String>>>> ();
	private Queue<KeyValuePair<String[], Dictionary<String, String>>> messagesQueue = new Queue<KeyValuePair<String[], Dictionary<String, String>>> ();
	private bool started = false;

	public void Awake () {
		if (instance == null) {
			instance = this;
			InstantiateSubs ();
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	public void Start () {
		started = true;
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
		messagesQueue.Enqueue (new KeyValuePair<string[], Dictionary<string, string>> (targets, message));
		SendQueueItems ();
	}

	private void SendQueueItems () {
		if (ChatService.GetInstance () == null || !ChatService.GetInstance ().connected || ! started) {
			return;
		}

		while (messagesQueue.Count != 0) {
			KeyValuePair<String[], Dictionary<String, String>> messageEntry = messagesQueue.Dequeue ();
			foreach (var target in messageEntry.Key) {
				if (!CurrentUser.GetInstance ().GetUserInfo ().username.Equals (target)) {
					Debug.LogWarning ("Send update of type " + GetData<UpdateType> (messageEntry.Value, "type") + " from " + CurrentUser.GetInstance ().GetUserInfo ().username + " to " + target);
					ChatService.GetInstance ().SendPrivateMessage (target, messageEntry.Value);
				}
			}
		}
		messagesQueue = new Queue<KeyValuePair<String[], Dictionary<String, String>>> ();
	}

	public IEnumerator Wait (float seconds) {
		yield return new WaitForSeconds (seconds);
	}

	public void Recieve (string sender, Dictionary<String, String> message) {
		lock (subscribers) {
			List<Action<String, Dictionary<String, String>>> functions = 
					subscribers[JsonUtility.FromJson<UpdateType> (message["type"])];
			for (int i = 0; i < functions.Count; i++) {
				Action<String, Dictionary<String, String>> func = functions [i];
				if (func == null || sender.Equals (CurrentUser.GetInstance ().GetUserInfo ().username)) {
					continue;
				}
				func (sender, message);
			}
		}
	}

	// returns unsubscribe function
	public Action Subscribe (UpdateType ev, Action<String, Dictionary<String, String>> func) {
		List<Action<String, Dictionary<String, String>>> functions;
		functions = subscribers [ev];
		functions.Add (func);
		
		return () => {
			functions.Remove (func);
		};
	}

	public static T GetData<T> (Dictionary<String, String> message, string key) {
		return JsonUtility.FromJson<T>(message[key]);
	}

	public static KeyValuePair<string, string> CreateKV<T> (string key, T value) {
		return new KeyValuePair<string, string> (key, JsonUtility.ToJson (value));
	}

	public void Update () {
//		SendQueueItems ();
	}
}
