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
	public const String APP_ID = "0b79eaae-0063-4f99-9212-ed71c61a6375";
	public bool connected = false;
	private static ChatService instance = null;
	private ChatClient chatClient = null;
	private String activeCH = GLOBAL_CH;
	private List<String> chatMessages = new List<String> ();
	private Action onFinish;

	public String GetPartyCHName () {
		return CurrentUser.GetInstance ().GetUserInfo ().username;
	}

	public void StartService (Action onFinish) {
		this.onFinish = onFinish;
		ConnectToChatService ();
	}

	public void StopService () {
		this.onFinish = null;
		connected = false;
	}

	public void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	public void Update () {
		if (chatClient != null) {
			chatClient.Service ();	
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

	private ChatController GetChat () {
		return GameObject.FindGameObjectWithTag ("Chat").GetComponent<ChatController> ();
	}

	public void ChangeChanel (String name) {
		activeCH = name;
		GetChat ().UpdateViewport (chatMessages, activeCH);
	}

	public void Unsubscribe (string[] chs) {
		chatClient.Unsubscribe (chs);
	}

	public void SendPrivateMessage (string target, object message) {
		while (!connected) { }
		chatClient.SendPrivateMessage (target, message);
	}

	public void SendTextMessage (String message) {
		if (!message.Equals ("")) {
			message = "[" + chatClient.UserId + "]: " + message;
			chatClient.PublishMessage (activeCH, message);
			chatMessages.Add (message);
			GetChat ().UpdateViewport (chatMessages, activeCH);
		}
	}

	public void CreateNewChat (String name) {
		chatClient.Subscribe (new String[]{name});
	}

	public void DebugReturn (DebugLevel level, string message) {
		Debug.Log ("DebugReturn: " + message);
	}

	public void OnDisconnected () {
		connected = false;
	}

	public void Subscribe (string[] chs) {
		chatClient.Subscribe (chs);
	}

	public void OnConnected () {
		GetChat ().InitDefaultChat ();
		connected = true;
		onFinish ();
	}

	public void OnChatStateChange(ChatState state) {
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages) {
		for (int i = 0; i < messages.Length; i++) {
			if (senders [i] == chatClient.UserId) {
				continue;
			}
			chatMessages.Add ((String) messages [i]);
		}
		GameObject.FindGameObjectWithTag ("ChatButtonsView").GetComponent<ChatTabController> ().ChatAlreadyExist(channelName);
		GetChat ().UpdateViewport (chatMessages, channelName);
	}

	public void OnPrivateMessage(string sender, object message, string channelName) {
		UpdateService.GetInstance ().Recieve (sender, (Dictionary<String, String>) message);
	}

	public void OnSubscribed(string[] channels, bool[] results) {
		
	}

	public void OnUnsubscribed(string[] channels) {
	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message) {
		
	}
}
