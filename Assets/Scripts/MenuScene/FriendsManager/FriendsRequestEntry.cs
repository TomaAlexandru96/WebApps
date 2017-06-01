using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsRequestEntry : MonoBehaviour {


	private GameObject friendsPanel;
	public GameObject friendRequestEntry;
	public Image avatar;


	public void Start() {
		friendsPanel = GameObject.FindGameObjectWithTag ("Friends");
	}

	public string GetName () {
		return friendRequestEntry.transform.GetChild (0).GetComponent<Text> ().text;
	}

	public void AcceptRequest() {
		DBServer.GetInstance ().AcceptFriendRequest (GetName (), () => {
			friendsPanel.transform.GetComponent<FriendsPanelManager> ().CreateFriend (GetName ());
			UpdateService.GetInstance ().SendUpdate (new string[]{GetName ()}, 
						UpdateService.CreateMessage (UpdateType.UserUpdate));
			Destroy (friendRequestEntry);
		}, (error) => {
			Debug.LogError (error);
		});
	}

	public void RejectRequest() {
		DBServer.GetInstance ().RejectFriendRequest (GetName (), () => {
			Destroy (friendRequestEntry);	
		}, (error) => {
			Debug.LogError (error);
		});
	}

	public void SetName (string name) {
		friendRequestEntry.transform.GetChild (0).GetComponent<Text> ().text = name;
		DBServer.GetInstance ().FindUser (name, (user) => {
			avatar.sprite = user.character.GetImage ();
		}, (error) => {
			Debug.LogError (error);
		});
	}
}
