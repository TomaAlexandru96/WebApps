using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyControl : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject addPlayer;

	private int maxPlayers = 4;
	private int playersInParty = 1;
	private string owner;

	public void Start () {
		owner = CurrentUser.GetInstance ().GetUserInfo ().username;
		AddPlayer (CurrentUser.GetInstance ().GetUserInfo ().username);
		UpdateService.GetInstance ().Subscribe (UpdateType.PartyRequest, (sender, message) => {
			ConfirmAlertController.Create ("You have received a party invite from " + sender, (alert) => {
				owner = sender;
				UpdateService.GetInstance ().SendUpdate (new string[]{owner}, UpdateService.CreateMessage (UpdateType.PartyRequestAccept));
				alert.Close ();
			}, (alert) => {
				alert.Close ();
			});
		});
	}

	private void RequestAddPlayer () {
		if (owner == CurrentUser.GetInstance ().GetUserInfo ().username) {
			RequestAlertController.Create("Who would you want to add to the party?", (controller, input) => {
				DBServer.GetInstance ().FindUser (input, (user) => {
					UpdateService.GetInstance ().SendUpdate (new string[]{user.username}, UpdateService.CreateMessage (UpdateType.PartyRequest));
					controller.Close ();
				}, (error) => {
					Debug.LogError (error);
				});
			});	
		}
	}

	public void AddPlayer(string username) {
		if (playersInParty < maxPlayers) {
			GameObject newPlayer = Instantiate (playerPrefab);
			newPlayer.transform.SetParent (transform);
			newPlayer.GetComponent<PartyEntry> ().ChangeName (username);
			playersInParty++;
		}
	}

	public void Update () {
		addPlayer.SetActive (owner == CurrentUser.GetInstance ().GetUserInfo ().username);
	}
}
