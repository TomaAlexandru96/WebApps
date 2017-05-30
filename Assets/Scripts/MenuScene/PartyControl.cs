using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyControl : MonoBehaviour {

	public GameObject playerPrefab;

	private int maxPlayers = 4;
	private int playersInParty = 1;

	public void Start () {
		AddPlayer (CurrentUser.GetInstance ().GetUserInfo ().username);
		UpdateService.GetInstance ().Subscribe (UpdateType.PartyRequest, () => {
			RequestAddPlayer ();
		});
	}

	private void RequestAddPlayer () {
		RequestAlertController.Create("Who would you wnat to add to the party?", (controller, input) => {
			DBServer.GetInstance ().FindUser (input, (user) => {
				UpdateService.GetInstance ().SendUpdate (new string[]{user.username}, UpdateType.PartyRequest);
				controller.Close ();
			}, (error) => {
				Debug.LogError (error);
			});
		});
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
