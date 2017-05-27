using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatController : MonoBehaviour {

	public GameObject messagePrefab;
	public GameObject chatPanelPrefab;
	public GameObject viewport;
	public InputField input;
	public ChatTabController chatTabController;

	private GameObject activePanel;
	private Dictionary<String, GameObject> allChatPanels = new Dictionary<String, GameObject> ();

	public void Start () {
		chatTabController.AddChat (ChatService.GLOBAL_CH);
		chatTabController.AddChat (ChatService.PARTY_CH);
	}

	public void Update () {
		if (Input.GetKeyUp (KeyCode.Return)) {
			SendMessage ();
		}
	}

	public void UpdateViewport (List<String> chatMessages) {
		while (chatMessages.Count != 0) {
			GameObject newMessageObj = Instantiate (messagePrefab);
			newMessageObj.transform.SetParent (activePanel.transform);
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

	public void CreateNewChat (String name) {
		ChatService.GetInstance ().CreateNewChat (name);
		GameObject chatPanel = Instantiate (chatPanelPrefab, Vector3.zero, Quaternion.identity);
		chatPanel.transform.SetParent (viewport.transform, false);
		allChatPanels.Add (name, chatPanel);
		LoadChat (name);
	}

	public void LoadChat (String name) {
		if (!allChatPanels.TryGetValue (name, out activePanel)) {
			Debug.LogError ("No chat named: " + name);
		}

		foreach (var obj in allChatPanels.Values) {
			obj.SetActive (false);
		}
			
		activePanel.SetActive (true);
		ChatService.GetInstance ().ChangeChanel (name);
	}
}
