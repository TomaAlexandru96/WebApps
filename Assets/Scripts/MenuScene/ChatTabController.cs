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
		AddChat ("chat " + totalChats);
	}

	public void AddChat (String name) {
		GameObject newTab = Instantiate (tabPrefab);
		newTab.transform.SetParent (content.transform);
		newTab.GetComponent <ChatTab> ().UpdateName (name, totalChats);
		totalChats++;
		chat.CreateNewChat (name);
		ActivateLastTab ();
	}

	private void ActivateLastTab() {
		for (int i = 0; i < GameObject.FindGameObjectWithTag ("ChatButtons").transform.childCount; i++) {
			GameObject.FindGameObjectWithTag ("ChatButtons").transform.GetChild(i).GetComponent<Image>().color = new Color32(255,255,225,240);
		}
		int totalTabs = GameObject.FindGameObjectWithTag ("ChatButtons").transform.childCount;
		GameObject.FindGameObjectWithTag ("ChatButtons").transform.GetChild(totalTabs-1).GetComponent<Image>().color = new Color32 (0, 0, 0, 0);
		getChatView ().GetComponent<ScrollRect> ().content = 
			(RectTransform)getChatView ().GetChild(0).transform.GetChild (totalTabs-1);
	}

	private Transform getChatView() {
		return GameObject.FindGameObjectWithTag ("Chat").transform.GetChild (0);
	} 
}
