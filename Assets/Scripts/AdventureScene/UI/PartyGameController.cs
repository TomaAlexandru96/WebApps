﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyGameController : MonoBehaviour {

	public GameObject playerPrefab;

	void Start () {
		foreach (var username in CurrentUser.GetInstance ().GetUserInfo ().party.partyMembers) {
			GameObject go = Instantiate (playerPrefab);
			go.GetComponent <PlayerGameUIController> ().SetStats (username);
			go.transform.SetParent (transform);
		}
	}
}