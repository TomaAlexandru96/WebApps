using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsTopButtonsManager : MonoBehaviour {

	public GameObject friendsPanel;
	public GameObject friendRequestsPanel;
	public FriendsPanelManager manager;

	public void ActivateFriendsPanel () {
		friendsPanel.SetActive (true);
		friendRequestsPanel.SetActive (false);
		manager.UpdatePanel (0.1f);
	}

	public void ActivateFriendRequestsPanel () {
		friendsPanel.SetActive (false);
		friendRequestsPanel.SetActive (true);
		manager.UpdatePanel (0.1f);
	}

	public void RequestSendFriendRequset () {
		RequestAlertController.Create ("Who do you want to add as a friend?", (alert, response) => {
			DBServer.GetInstance ().FindUser (response, (user) => {
				DBServer.GetInstance ().RequestFriend (user.username, () => {
					UpdateService.GetInstance ().SendUpdate (new string[]{user.username}, 
							UpdateService.CreateMessage (UpdateType.UserUpdate));
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
