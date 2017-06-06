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
	public bool withFadeOut;

	private string activeChat;
	private Dictionary<String, GameObject> allChatPanels = new Dictionary<String, GameObject> ();
	private Action unsub;

	public void Awake () {
		unsub = UpdateService.GetInstance ().Subscribe (UpdateType.ChatMessage, (sender, message) => {
			UpdateViewport (UpdateService.GetData (message, "value"), GetPrivateChatName (sender));
		});
	}

	public void OnDestroy () {
		unsub ();
	}

	public void InitDefaultChat () {
		foreach (var chat in CurrentUser.GetInstance ().GetSubscribedCH ()) {
			bool isNotCloseable = chat.Equals (ChatService.GLOBAL_CH) || 
				(CurrentUser.GetInstance ().IsInParty () && CurrentUser.GetInstance ().GetUserInfo ().party.owner.Equals (chat));
			CreateNewChat (chat, GetChatViewName (chat), !isNotCloseable);
		}
	}

	public void Start () {
		if (withFadeOut) {
			GetComponent<CanvasGroup> ().alpha = 0.2f;
		}
	}

	public void Update () {
		if (Input.GetKeyUp (KeyCode.Return)) {
			SendMessage ();
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		if (withFadeOut) {
			GetComponent<CanvasGroup> ().alpha = 1;	
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		if (withFadeOut) {
			GetComponent<CanvasGroup> ().alpha = 0.2f;
		}
	}

	public void UpdateViewport (List<String> chatMessages, string channel) {
		while (chatMessages.Count != 0) {
			UpdateViewport (chatMessages [0], channel);
			chatMessages.RemoveAt (0);
		}
	}

	private void UpdateViewport (string chatMessages, string channel) {
		if (IsPrivateChat (channel)) {
			CreateNewPrivateChat (OtherPersonFromPrivate (channel));
		}

		GameObject resultPanel;

		if (!allChatPanels.TryGetValue (channel, out resultPanel)) {
			Debug.LogError ("No chat named: " + channel);
		}

		GameObject newMessageObj = (GameObject) Instantiate (messagePrefab);
		newMessageObj.transform.SetParent (resultPanel.transform);
		newMessageObj.GetComponentInChildren<Text> ().text = chatMessages;
	}

	public void SendMessage () {
		if (IsPrivateChat (activeChat) && !input.text.Equals (string.Empty)) {
			UpdateService.GetInstance ().SendUpdate (new string[] {OtherPersonFromPrivate (activeChat)}, 
				UpdateService.CreateMessage (UpdateType.ChatMessage, UpdateService.CreateKV ("value", ChatService.GetInstance ().GetMessageTemplate (input.text))));
			UpdateViewport (ChatService.GetInstance ().GetMessageTemplate (input.text), activeChat);
		} else {
			ChatService.GetInstance ().SendTextMessage (input.text);			
		}
		
		input.text = "";
		input.Select ();
		input.ActivateInputField ();	
	}

	public void CreateNewChat (String name, String viewName, bool isCloseable) {
		if (chatTabController.ChatAlreadyExist (name)) {
			return;
		}
		if (!IsPrivateChat (name)) {
			ChatService.GetInstance ().CreateNewChat (name);	
		}
		CurrentUser.GetInstance ().SubscribeToCH (name);
		GameObject chatPanel = (GameObject) Instantiate (chatPanelPrefab, Vector3.zero, Quaternion.identity);
		chatPanel.transform.SetParent (viewport.transform, false);
		allChatPanels.Add (name, chatPanel);
		chatTabController.AddChat (name, viewName, isCloseable);
	}

	public void CreateNewPrivateChat (String other) {
		CreateNewChat (GetPrivateChatName (other), other, true);
	}

	public void DestroyChat (string name) {
		if (!IsPrivateChat (name)) {
			ChatService.GetInstance ().Unsubscribe (new string[]{ name });
		}
		CurrentUser.GetInstance ().UnsubscribeCH (name);
		chatTabController.DestroyChat (name);
		allChatPanels.Remove (name);
	}

	public void LoadChat (String name) {
		GameObject activePanel;
		if (!allChatPanels.TryGetValue (name, out activePanel)) {
			Debug.LogError ("No chat named: " + name);
			return;
		}

		foreach (var obj in allChatPanels.Values) {
			obj.SetActive (false);
		}

		activePanel.SetActive (true);
		activeChat = name;
		GetScrollRect ().content = activePanel.GetComponent<RectTransform> ();
		ChatService.GetInstance ().ChangeChanel (name);
	}

	private ScrollRect GetScrollRect () {
		return transform.GetChild (0).GetComponent <ScrollRect> ();
	}

	private string GetPrivateChatName (String other) {
		String current = CurrentUser.GetInstance ().GetUserInfo ().username;

		if (current.CompareTo (other) > 0) {
			return other + ":" + current;
		} else {
			return current + ":" + other;
		}
	}

	public void InviteToChat () {
		RequestAlertController.Create ("Who do you want to chat with?", (alert, input) => {
			if (CurrentUser.GetInstance ().GetUserInfo ().username.Equals (input)) {
				return;
			}
			DBServer.GetInstance ().FindUser (input, (user) => {
				CreateNewPrivateChat (user.username);
				alert.Close ();
			}, (error) => {
				Debug.LogError (error);
			});	
		});
	}

	public static ChatController GetChat () {
		return GameObject.FindGameObjectWithTag ("Chat").GetComponent <ChatController> ();
	}

	public string GetChatViewName (string name) {
		if (IsPrivateChat (name)) {
			return OtherPersonFromPrivate (name);
		} else if (!name.Equals (ChatService.GLOBAL_CH)) {
			return "Party";
		} else {
			return name;
		}
	}

	private bool IsPrivateChat (string name) {
		return name.Contains (":");
	}

	private string OtherPersonFromPrivate (string name) {
		string[] res = name.Split (new char[] {':'}, 2);
		if (res [0].Equals (CurrentUser.GetInstance ().GetUserInfo ().username)) {
			return res [1];
		} else {
			return res [0];
		}
	}
}
