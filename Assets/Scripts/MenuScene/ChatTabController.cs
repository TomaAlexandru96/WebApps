using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTabController : MonoBehaviour {

	public GameObject tabPrefab;
	public ChatController chat;
	public GameObject content;
	private int totalChats = -1;

	public void AddChat () {
		RequestAlertController.Create ("Who do you want to chat with?", (alert, input) => {
			DBServer.GetInstance ().FindUser (input, (user) => {
				if (user.active && !ChatAlreadyExist(CurrentUser.GetInstance().GetUserInfo().username + ':' + input)) {
					UpdateService.GetInstance ().SendUpdate (new string[]{input}, UpdateService.CreateMessage (UpdateType.ChatRequest));
					// AddChat ("Chat " + totalChats, true);
				}
				alert.Close ();
			}, (error) => {
				Debug.LogError (error);
			});	
		});
	}

	public void AddChat (String name, bool isCloseable) {
		if (ChatAlreadyExist (name)) {
			return;
		}
		GameObject newTab = (GameObject) Instantiate (tabPrefab);
		newTab.transform.SetParent (content.transform);
		newTab.GetComponent <ChatTab> ().UpdateName (name, totalChats);
		newTab.GetComponent <ChatTab> ().isCloseable = isCloseable;
		totalChats++;
		chat.CreateNewChat (name);
		ActivateLastTab ();
	}

	private void ActivateLastTab() {
		int totalTabs = GameObject.FindGameObjectWithTag ("ChatButtons").transform.childCount;
		GameObject.FindGameObjectWithTag ("ChatButtons").transform.GetChild (totalTabs - 1).GetComponent<ChatTab> ().SelectChat ();
	}

	public void DestroyChat (string chatName) {
		foreach (var tab in content.transform.GetComponentsInChildren<ChatTab> ()) {
			if (tab.GetName ().Equals (chatName)) {
				chat.DestroyChat (chatName);
				DestroyImmediate (tab.gameObject);
				break;
			}
		}
		ActivateLastTab ();
	}

	public bool ChatAlreadyExist(String name) {
		String chatName;
		String[] chatNames;
		String[] names = name.Split (':');
		for (int i = 0; i < content.transform.childCount; i++) {
			chatName = content.transform.GetChild (i).GetChild (0).GetComponent<Text> ().text;
			chatNames = chatName.Split (':');
			if (chatName.Equals (name)){
				content.transform.GetChild (i).gameObject.SetActive (true);
				return true;
			}
			if (chatNames.Length > 1) {
				if (chatNames[1].Equals(names[0]) && names[1].Equals(chatNames[0])) {
					content.transform.GetChild (i).gameObject.SetActive (true);
					return true;
				}
			}
		}
		return false;
	}
}
