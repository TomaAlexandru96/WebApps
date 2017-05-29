using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyControl : MonoBehaviour {

	public GameObject playerPrefab;

	private int maxPlayers = 4;
	private int playersInParty = 1;

	public void Start () {
		AddPlayer (CurrentUser.GetInstance ().GetUserInfo ().username);
	}

	public void AddPlayer(string username) {
		if (playersInParty < maxPlayers) {
			GameObject newPlayer = Instantiate (playerPrefab);
			newPlayer.transform.SetParent (transform);
			newPlayer.GetComponent<PartyEntry> ().ChangeName (username);
			playersInParty++;
		}
	} 
}
