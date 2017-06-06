using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyGameController : Photon.MonoBehaviour {

	public GameObject playerPrefab;

	void Start () {
		transform.SetParent (GameObject.FindGameObjectWithTag ("Canvas").transform, false);
		foreach (var username in CurrentUser.GetInstance ().GetUserInfo ().party.partyMembers) {
			GameObject go = Instantiate (playerPrefab);
			go.GetComponent <PlayerGameUIController> ().SetStats (username);
			go.transform.SetParent (transform);
		}
	}
}
