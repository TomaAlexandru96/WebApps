﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsPanelManager : MonoBehaviour {

	private GameObject friendsPanelContent;
	private GameObject friendsRequestPanel;

	public GameObject friendsEntry;
	public GameObject friendRequestEntry;
	private Action unsub;

	public void Awake () {
		unsub = UpdateService.GetInstance ().Subscribe (UpdateType.UserUpdate, (sender, message) => {
			GetAllFriends ();
			GetAllFriendsRequests ();
		});
	}

	public void Start () {
		friendsPanelContent = GameObject.FindGameObjectWithTag ("Friends").transform.GetChild (1).GetChild(0).GetChild(0).gameObject;
		friendsRequestPanel = GameObject.FindGameObjectWithTag ("Friends").transform.GetChild (2).GetChild(0).GetChild(0).gameObject;
	}

	public void OnDestroy () {
		unsub ();
	}

	public void CreateFriend (string name, GameObject image) {
		GameObject newFriendEntry = Instantiate (friendsEntry, Vector3.zero, Quaternion.identity);
		newFriendEntry.transform.SetParent (friendsPanelContent.transform, false);
		newFriendEntry.GetComponent<FriendsEntry> ().SetName (name);
		//image.transform.SetParent (newFriendEntry.transform, false);
	}

	public void CreateFriendRequest (string name, GameObject image) {
		GameObject newfriendRequestEntry = Instantiate (friendRequestEntry, Vector3.zero, Quaternion.identity);
		newfriendRequestEntry.transform.SetParent (friendsRequestPanel.transform, false);
		newfriendRequestEntry.transform.GetChild (0).GetComponent<Text> ().text = name;
		//image.transform.SetParent (newfriendRequestEntry.transform, false);
	}
		
	public void GetAllFriends () {
		foreach (Transform child in friendsPanelContent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		String[] friends = CurrentUser.GetInstance().GetUserInfo ().friends;
		foreach (var friend in friends) {
			CreateFriend (friend, null);
		}
	}

	public void GetAllFriendsRequests () {
		foreach (Transform child in friendsRequestPanel.transform) {
			GameObject.Destroy(child.gameObject);
		}
		String[] friend_requests = CurrentUser.GetInstance().GetUserInfo ().friend_requests;
		foreach (var f_r in friend_requests) {
			CreateFriendRequest (f_r, null);
		}
	}
}
