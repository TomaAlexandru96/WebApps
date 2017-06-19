using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionScript : MonoBehaviour {

	public GameObject inviteToPartyButton;
	private string playerName;

	public void Start () {
		GameObject.FindGameObjectWithTag ("FriendsOption").SetActive (false);
	}

	public void Update () {
		inviteToPartyButton.SetActive (CurrentUser.GetInstance ().IsInParty ());
	}

	public string GetPlayerName () {
		return playerName;
	}

	public void SetPlayerName (string playerName) {
		this.playerName = playerName;
	}

	public void InviteToParty() {
		gameObject.SetActive (false);
		DBServer.GetInstance ().FindUser (playerName, (user) => {
			if (!CurrentUser.GetInstance ().GetUserInfo ().party.ContainsPlayer (user.username) && user.active) {
				UpdateService.GetInstance ().SendUpdate (new string[]{user.username}, UpdateService.CreateMessage (UpdateType.PartyRequest, 
					UpdateService.CreateKV ("party_type", CurrentUser.GetInstance ().GetUserInfo ().party.state.ToString ())));
			}
		}, (error) => {
			Debug.LogError (error);
		});
	}

	public void InviteToChat() {
		DBServer.GetInstance ().FindUser (playerName, (user) => {
			ChatController.GetChat ().CreateNewPrivateChat (user.username);
			gameObject.SetActive (false);
		}, (error) => {
			Debug.LogError (error);
		});
	}
}
