using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsPanelManager : MonoBehaviour, Notifiable {

	private GameObject friendsPanelContent;
	private GameObject friendsRequestPanel;

	public GameObject friendsEntry;
	public GameObject friendRequestEntry;

	public void Start () {
		friendsPanelContent = GameObject.FindGameObjectWithTag ("Friends").transform.GetChild (1).GetChild(0).GetChild(0).gameObject;
		friendsRequestPanel = GameObject.FindGameObjectWithTag ("Friends").transform.GetChild (2).GetChild(0).GetChild(0).gameObject;

		Notify ();
		CurrentUser.GetInstance ().Subscribe (this);
	}

	public void CreateFriend (string name, GameObject image) {
		GameObject newFriendEntry = Instantiate (friendsEntry, Vector3.zero, Quaternion.identity);
		newFriendEntry.transform.SetParent (friendsPanelContent.transform, false);
		newFriendEntry.transform.GetChild (0).GetComponent<Text> ().text = name;
		Destroy (newFriendEntry.transform.GetChild (1).gameObject);
		//image.transform.SetParent (newFriendEntry.transform, false);
	}

	public void CreateFriendRequest (string name, GameObject image) {
		GameObject newfriendRequestEntry = Instantiate (friendRequestEntry, Vector3.zero, Quaternion.identity);
		newfriendRequestEntry.transform.SetParent (friendsRequestPanel.transform, false);
		newfriendRequestEntry.transform.GetChild (0).GetComponent<Text> ().text = name;
		Destroy (newfriendRequestEntry.transform.GetChild (1).gameObject);
		//image.transform.SetParent (newfriendRequestEntry.transform, false);
	}

	public void Notify () {
		GetAllFriends ();
		GetAllFriendsRequests ();
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
