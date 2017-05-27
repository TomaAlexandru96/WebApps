using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ExitGames.Client.Photon.Chat;
using ExitGames.Client.Photon;

public class ChatService : MonoBehaviour, IChatClientListener {

	public const String GLOBAL_CH = "General";
	public const String PARTY_CH = "Party";
	public const String APP_ID = "0b79eaae-0063-4f99-9212-ed71c61a6375";
	private bool Connected = false;
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

	public void ConnectToChatService () {
		chatClient = new ChatClient (this);
		ExitGames.Client.Photon.Chat.AuthenticationValues av = new ExitGames.Client.Photon.Chat.AuthenticationValues ();
		av.UserId = CurrentUser.GetInstance ().GetUserInfo ().username;
		while (!Connected) {
			Connected = chatClient.Connect (APP_ID, NetworkService.GAME_VERSION, av);
		}
	}

	private ChatController GetChat () {
		return GameObject.FindGameObjectWithTag ("Chat").GetComponent<ChatController> ();
	}

	public void ChangeChanel (String name) {
		activeCH = name;
		GetChat ().UpdateViewport (chatMessages);
	}

	public void SendMessage (String message) {
		if (!message.Equals ("")) {
			message = "[" + chatClient.UserId + "]: " + message;
			chatClient.PublishMessage (activeCH, message);
			chatMessages.Add (message);
			GetChat ().UpdateViewport (chatMessages);
		}
	}

	public void CreateNewChat (String name) {
		chatClient.Subscribe (new String[]{name});
	}

	public void DebugReturn(DebugLevel level, string message) {
		Debug.Log ("DebugReturn: " + message);
	}

	public void OnDisconnected() {
		
	}

	public void OnConnected() {
		CreateNewChat (GLOBAL_CH);
		CreateNewChat (PARTY_CH);
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
