using System;
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
			UpdatePanel (1f);
		});
	}

	public void UpdatePanel (float waitTime) {
		CurrentUser.GetInstance ().RequestUpdate ((user) => {
			GetAllFriends ();
			GetAllFriendsRequests ();	
		});
	}

	public void Start () {
		friendsPanelContent = GameObject.FindGameObjectWithTag ("Friends").transform.GetChild (1).GetChild(0).GetChild(0).gameObject;
		friendsRequestPanel = GameObject.FindGameObjectWithTag ("Friends").transform.GetChild (2).GetChild(0).GetChild(0).gameObject;
		UpdatePanel (0f);
	}

	public void OnDestroy () {
		unsub ();
	}

	public void CreateFriend (string name) {
		GameObject newFriendEntry = (GameObject) Instantiate (friendsEntry, Vector3.zero, Quaternion.identity);
		newFriendEntry.transform.SetParent (friendsPanelContent.transform, false);
		newFriendEntry.GetComponent<FriendsEntry> ().SetName (name);
	}

	public void CreateFriendRequest (string name) {
		GameObject newfriendRequestEntry = (GameObject) Instantiate (friendRequestEntry, Vector3.zero, Quaternion.identity);
		newfriendRequestEntry.transform.SetParent (friendsRequestPanel.transform, false);
		newfriendRequestEntry.GetComponent<FriendsRequestEntry> ().SetName (name);
	}
		
	public void GetAllFriends () {
		String[] friends = CurrentUser.GetInstance().GetUserInfo ().friends;
		foreach (var friend in friends) {
			if (!IsFriendInPanel (friend)) {
				CreateFriend (friend);
			}
		}
	}

	private bool IsFriendInPanel (string friend) {
		foreach (var entry in friendsPanelContent.transform.GetComponentsInChildren<FriendsEntry> ()) {
			if (entry.GetName ().Equals (friend)) {
				return true;
			}
		}
		return false;
	}

	private bool IsFriendRequestInPanel (string friend) {
		foreach (var entry in friendsRequestPanel.transform.GetComponentsInChildren<FriendsRequestEntry> ()) {
			if (entry.GetName ().Equals (friend)) {
				return true;
			}
		}
		return false;
	}

	public void GetAllFriendsRequests () {
		String[] friend_requests = CurrentUser.GetInstance().GetUserInfo ().friend_requests;
		foreach (var f_r in friend_requests) {
			if (!IsFriendRequestInPanel (f_r)) {
				CreateFriendRequest (f_r);
			}
		}
	}
}
