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
		AddChat ("chat " + totalChats, true);
	}

	public void AddChat (String name, bool isCloseable) {
		GameObject newTab = Instantiate (tabPrefab);
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
}
