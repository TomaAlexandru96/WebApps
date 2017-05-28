using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsRequestEntry : MonoBehaviour {


	private GameObject friendsPanel;
	public GameObject friendRequestEntry;


	public void Start() {
		friendsPanel = GameObject.FindGameObjectWithTag ("Friends");
	}

	public void AcceptRequest() {
		string name = friendRequestEntry.transform.GetChild (0).GetComponent<Text> ().text;
		GameObject image = Instantiate (friendRequestEntry.transform.GetChild (1).gameObject, Vector3.zero, Quaternion.identity);

		friendsPanel.transform.GetComponent<FriendsPanelManager> ().CreateFriend(name,image);
		DestroyImmediate (friendRequestEntry);
	}

	public void RejectRequest() {
		DestroyImmediate (friendRequestEntry);
	}
}
