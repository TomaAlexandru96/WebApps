﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsRequestEntry : MonoBehaviour {


	private GameObject friendsPanel;
	public GameObject friendRequestEntry;


	public void Start() {
		friendsPanel = GameObject.FindGameObjectWithTag ("Friends");
	}

	private string GetName () {
		return friendRequestEntry.transform.GetChild (0).GetComponent<Text> ().text;
	}

	public void AcceptRequest() {
		DBServer.GetInstance ().AcceptFriendRequest (GetName (), () => {
			GameObject image = Instantiate (friendRequestEntry.transform.GetChild (1).gameObject, Vector3.zero, Quaternion.identity);

			friendsPanel.transform.GetComponent<FriendsPanelManager> ().CreateFriend (GetName (), image);
			DestroyImmediate (friendRequestEntry);
		}, (error) => {
			Debug.LogError (error);
		});
	}

	public void RejectRequest() {
		DBServer.GetInstance ().RejectFriendRequest (GetName (), () => {
			DestroyImmediate (friendRequestEntry);	
		}, (error) => {
			Debug.LogError (error);
		});
	}
}