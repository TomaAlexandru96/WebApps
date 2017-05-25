using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ExitGames.Client.Photon.Chat;
using ExitGames.Client.Photon;

public class ChatService : MonoBehaviour, IChatClientListener {

	public const String APP_ID = "0b79eaae-0063-4f99-9212-ed71c61a6375";
	public const String GLOBAL_CH = "General";
	public const int FONT_SIZE = 20;
	public InputField input;
	public Text viewportContentText;
	public GameObject viewportContent;
	private static ChatService instance = null;
	private ChatClient chatClient = null;
	private String activeCH = GLOBAL_CH;
	private List<String> chatMessages = new List<String> ();

	void Awake () {
		if (instance == null) {
			instance = this;
			ConnectToChatService ();
		} else {
			Destroy (gameObject);
		}
	}

	public void Update () {
		chatClient.Service ();
		if (EventSystem.current.currentSelectedGameObject == input.gameObject && 
					Input.GetKeyUp (KeyCode.Return)) {
			SendMessage ();
		}
	}

	public static ChatService GetInstance () {
		return instance;
	}

	private void ConnectToChatService () {
		chatClient = new ChatClient (this);
		ExitGames.Client.Photon.Chat.AuthenticationValues av = new ExitGames.Client.Photon.Chat.AuthenticationValues ();
		av.UserId = CurrentUser.GetInstance ().GetUserInfo ().username;
		chatClient.Connect (APP_ID, NetworkService.GAME_VERSION, av);
	}

	public void SendMessage () {
		if (!input.text.Equals ("")) {
			chatClient.PublishMessage (activeCH, input.text);
			String message = "[" + chatClient.UserId + "]: " + input.text + "\n";
			chatMessages.Add (message);
			UpdateViewport ();
			input.text = "";
		}
	}

	public void DebugReturn(DebugLevel level, string message) {
		Debug.Log ("DebugReturn: " + message);
	}

	public void OnDisconnected() {
		
	}

	public void OnConnected() {
		// initalise global chat
		chatClient.Subscribe (new String[]{GLOBAL_CH});
	}

	public void OnChatStateChange(ChatState state) {
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages) {
		for (int i = 0; i < messages.Length; i++) {
			if (senders [i] == chatClient.UserId) {
				continue;
			}

			String message = "[" + senders[i] + "]: " + messages [i] + "\n";
			chatMessages.Add (message);
		}

		UpdateViewport ();
	}

	private void UpdateViewport () {
		while (chatMessages.Count != 0) {
			viewportContentText.text += chatMessages [0];
			chatMessages.RemoveAt (0);
			viewportContent.GetComponent <RectTransform> ().sizeDelta += new Vector2 (0, FONT_SIZE);
		}
	}

	public void OnPrivateMessage(string sender, object message, string channelName) {
		
	}

	public void OnSubscribed(string[] channels, bool[] results) {
		
	}

	public void OnUnsubscribed(string[] channels) {
		
	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message) {
		
	}
}
