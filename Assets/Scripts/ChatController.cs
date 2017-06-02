using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public GameObject messagePrefab;
	public GameObject chatPanelPrefab;
	public GameObject viewport;
	public InputField input;
	public ChatTabController chatTabController;

	private GameObject activePanel;
	private Dictionary<String, GameObject> allChatPanels = new Dictionary<String, GameObject> ();

	public Action unsub7;
	public Action unsub8;

	public void InitDefaultChat () {
		chatTabController.AddChat (ChatService.GLOBAL_CH, false);
	}

	public void Start () {
		GetComponent<CanvasGroup> ().alpha = 0.2f;
	}

	public void Update () {
		if (Input.GetKeyUp (KeyCode.Return)) {
			SendMessage ();
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		GetComponent<CanvasGroup> ().alpha = 1;
	}

	public void OnPointerExit(PointerEventData eventData) {
		GetComponent<CanvasGroup> ().alpha = 0.2f;
	}

	public void UpdateViewport (List<String> chatMessages, string channel) {
		GameObject resultPanel;
		if (!allChatPanels.TryGetValue (channel, out resultPanel)) {
			Debug.LogError ("No chat named: " + channel);
		}

		while (chatMessages.Count != 0) {
			GameObject newMessageObj = (GameObject) Instantiate (messagePrefab);
			newMessageObj.transform.SetParent (resultPanel.transform);
			newMessageObj.GetComponentInChildren<Text> ().text = chatMessages [0];
			chatMessages.RemoveAt (0);
		}
	}

	public void SendMessage () {
		ChatService.GetInstance ().SendTextMessage (input.text);
		input.text = "";
		input.Select ();
		input.ActivateInputField ();	
	}

	public void CreateNewChat (String name) {
		ChatService.GetInstance ().CreateNewChat (name);
		GameObject chatPanel = (GameObject) Instantiate (chatPanelPrefab, Vector3.zero, Quaternion.identity);
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

	public void  Awake () {
		unsub7 = UpdateService.GetInstance ().Subscribe (UpdateType.ChatRequest, (sender, message) => {
			ConfirmAlertController.Create ("You have received a chat invite from " + sender, (alert) => {
//				if(!partyMembers.ContainsPlayer(sender)){
				UpdateService.GetInstance ().SendUpdate (new string[]{sender}, 
						UpdateService.CreateMessage (UpdateType.ChatRequestAccept));
					
				chatTabController.AddChat (CurrentUser.GetInstance().GetUserInfo().username + ":" + sender, true);
//				} else {
//					Debug.Log("Duplicate invite");
//				}
				alert.Close ();
			}, (alert) => {
				alert.Close ();
			});
		});

		unsub8 = UpdateService.GetInstance ().Subscribe (UpdateType.ChatRequestAccept, (sender, message) => {
			chatTabController.AddChat (sender + ":" + CurrentUser.GetInstance().GetUserInfo().username, true);
		});
	}

	public void OnDestroy() {
		unsub7 ();
		unsub8 ();
	}
}
