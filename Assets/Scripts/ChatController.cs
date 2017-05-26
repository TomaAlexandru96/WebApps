using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatController : MonoBehaviour {
	
	public GameObject messagePrefab;
	public GameObject content;
	public InputField input;

	public void Update () {
		if (Input.GetKeyUp (KeyCode.Return)) {
			SendMessage ();
		}
	}

	public void UpdateViewport (List<String> chatMessages) {
		while (chatMessages.Count != 0) {
			GameObject newMessageObj = Instantiate (messagePrefab);
			newMessageObj.transform.SetParent (content.transform);
			newMessageObj.GetComponentInChildren<Text> ().text = chatMessages [0];
			chatMessages.RemoveAt (0);
		}
	}

	public void SendMessage () {
		ChatService.GetInstance ().SendMessage (input.text);
		input.text = "";
		input.Select ();
		input.ActivateInputField ();	
	}
}
