using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatService : MonoBehaviour {

	private static ChatService instance = null;

	void Awake () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	public static ChatService GetInstance () {
		return instance;
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("Chat intialised");
	}
}
