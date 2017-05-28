using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsPanelManager : MonoBehaviour {

	private GameObject friendsPanelContent;
	private GameObject friendsRequestPanel;
	private string name;
	private GameObject image;

	public GameObject friendsEntry;
	public GameObject friendRequestEntry;

	public void Start() {
		friendsPanelContent = GameObject.FindGameObjectWithTag ("Friends").transform.GetChild (1).GetChild(0).GetChild(0).gameObject;
		friendsRequestPanel = GameObject.FindGameObjectWithTag ("Friends").transform.GetChild (2).GetChild(0).GetChild(0).gameObject;
	}

	public void CreateFriend(string name, GameObject image) {
		setName (name, image);
		CreateFriend ();
	}

	public void CreateFriend() {
		GameObject newFriendEntry = Instantiate (friendsEntry, Vector3.zero, Quaternion.identity);
		newFriendEntry.transform.SetParent (friendsPanelContent.transform, false);
		newFriendEntry.transform.GetChild (0).GetComponent<Text> ().text = name;
		Destroy (newFriendEntry.transform.GetChild (1).gameObject);
		image.transform.SetParent (newFriendEntry.transform, false);
	}

	public void CreateFriendRequest(string name, GameObject image) {
		setName (name, image);
		CreateFriendRequest ();
	}

	public void CreateFriendRequest() {
		GameObject newfriendRequestEntry = Instantiate (friendRequestEntry, Vector3.zero, Quaternion.identity);
		newfriendRequestEntry.transform.SetParent (friendsRequestPanel.transform, false);
		newfriendRequestEntry.transform.GetChild (0).GetComponent<Text> ().text = name;
		Destroy (newfriendRequestEntry.transform.GetChild (1).gameObject);
		image.transform.SetParent (newfriendRequestEntry.transform, false);
	}

	public void setName(string name, GameObject image) {
		this.name = name;
		this.image = image; 
	}

}
