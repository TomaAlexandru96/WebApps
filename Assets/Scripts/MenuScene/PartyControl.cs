using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyControl : MonoBehaviour {

	public GameObject playerPrefab;

	private int maxPlayers = 4;
	private int playersInParty = 1;

	public void AddPlayer() {
		if (playersInParty < maxPlayers) {
			GameObject newPlayer = Instantiate (playerPrefab);
			newPlayer.transform.SetParent (transform);
			playersInParty++;
		}
	} 
}
