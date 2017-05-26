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
	private static ChatService instance = null;
	private ChatClient chatClient = null;
	private String activeCH = GLOBAL_CH;
	private List<String> chatMessages = new List<String> ();

	public void Awake () {
		if (instance == null) {
			instance = this;
			ConnectToChatService ();
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	public void Update () {
		chatClient.Service ();
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

	private ChatController GetChat () {
		return GameObject.FindGameObjectWithTag ("Chat").GetComponent<ChatController> ();
	}

	public void SendMessage (String message) {
		if (!message.Equals ("")) {
			message = "[" + chatClient.UserId + "]: " + message;
			chatClient.PublishMessage (activeCH, message);
			chatMessages.Add (message);
			GetChat ().UpdateViewport (chatMessages);
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

			String message = "[" + senders[i] + "]: " + messages [i];
			chatMessages.Add (message);
		}

		GetChat ().UpdateViewport (chatMessages);
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
