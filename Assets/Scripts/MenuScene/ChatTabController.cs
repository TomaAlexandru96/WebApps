﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTabController : MonoBehaviour {

	public GameObject tabPrefab;
	public ChatController chat;
	public GameObject content;
	private int totalChats = 0;

	public void AddChat () {
		AddChat ("test");
	}

	public void AddChat (String name) {
		GameObject newTab = Instantiate (tabPrefab);
		newTab.transform.SetParent (content.transform);
		newTab.GetComponent <ChatTab> ().UpdateName (name);
		totalChats++;
		chat.CreateNewChat (name);
	}
}