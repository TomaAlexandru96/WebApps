using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyControl : MonoBehaviour {

	public GameObject player;
	public GameObject party;

	private int maxPlayers = 4;
	private int playersInParty = 1;

	public void addPlayer() {
		if (playersInParty < maxPlayers) {
			GameObject newPlayer = Instantiate (player, party.transform);
			Vector3 position = player.GetComponent<RectTransform> ().localPosition;
			newPlayer.GetComponent<RectTransform> ().localPosition = new Vector3 (position.x, position.y - 70, position.z);


			player = newPlayer;
			playersInParty++;
		}
	} 
}
