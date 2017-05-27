using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsTopButtonsManager : MonoBehaviour {

	public GameObject friendsPanel;
	public GameObject friendRequestsPanel;

	public void ActivateFriendsPanel () {
		friendsPanel.SetActive (true);
		friendRequestsPanel.SetActive (false);
	}

	public void ActivateFriendRequestsPanel () {
		friendsPanel.SetActive (false);
		friendRequestsPanel.SetActive (true);
	}

	public void RequestSendFriendRequset () {
		RequestAlertController.Create ("Who do you want to add as a friend?", (alert, response) => {
			DBServer.GetInstance ().FindUser (response, (user) => {
				Debug.Log (user);
				alert.Close ();
			}, (error) => {
				Debug.LogError ("Failed: " + error);
			});
		});
	}
}
