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
				DBServer.GetInstance ().RequestFriend (user.username, () => {
					alert.Close ();	
				}, (error) => {
					Debug.LogError ("Failed Friend Request: " + error);	
				});
			}, (error) => {
				Debug.LogError ("Failed: " + error);
			});
		});
	}
}
