using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsPanelManager : MonoBehaviour {

	private GameObject friendsPanelContent;
	public GameObject friendsEntry;
	public GameObject friendRequestEntry;

	public void Start() {
		friendsPanelContent = GameObject.FindGameObjectWithTag ("Friends").transform.GetChild (1).GetChild(0).GetChild(0).gameObject;
	}

	public void AcceptRequest() {
		GameObject newFriendEntry = Instantiate (friendsEntry, Vector3.zero, Quaternion.identity);
		newFriendEntry.transform.SetParent (friendsPanelContent.transform, false);
		string name = friendRequestEntry.transform.GetChild (0).GetComponent<Text> ().text;
		newFriendEntry.transform.GetChild (0).GetComponent<Text> ().text = name;
		GameObject image = Instantiate (friendRequestEntry.transform.GetChild (1).gameObject, Vector3.zero, Quaternion.identity);
		Destroy (newFriendEntry.transform.GetChild (1).gameObject);
		image.transform.SetParent (newFriendEntry.transform, false);
		DestroyImmediate (friendRequestEntry);
	}

	public void RejectRequest() {
		DestroyImmediate (friendRequestEntry);
	}

}
