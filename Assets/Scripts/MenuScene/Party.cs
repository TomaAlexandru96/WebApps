using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Party : MonoBehaviour {

	public const int maxSize = 4;

	public GameObject playerPrefab;
	public GameObject addPlayer;
	public GameObject leaveParty;
	public Text gameModeLabel;
	public MenuController menuController;
	public ChatTabController tabController;

	private string partyChatName;
	private Action unsub2;
	private Action unsub3;
	private Action unsub4;

	public void Awake () {
		unsub2 = UpdateService.GetInstance ().Subscribe (UpdateType.PartyRequestAccept, (sender, message) => {
			UpdateParty ();
		});

		unsub3 = UpdateService.GetInstance ().Subscribe (UpdateType.LogoutUser, (sender, message) => {
			UpdateParty ();
		});

		unsub4 = UpdateService.GetInstance ().Subscribe (UpdateType.PartyLeft, (sender, message) => {
			UpdateParty ();
		});
	}

	public void OnDestroy () {
		unsub2 ();
		unsub3 ();
		unsub4 ();
	}

	public void RequestAddPlayer () {
		var owner = CurrentUser.GetInstance ().GetUserInfo ().party.owner;
		var partyMembers = CurrentUser.GetInstance ().GetUserInfo ().party;

		if (owner == CurrentUser.GetInstance ().GetUserInfo ().username && partyMembers.GetSize () < maxSize) {
			RequestAlertController.Create("Who would you want to add to the party?", (controller, input) => {
				DBServer.GetInstance ().FindUser (input, (user) => {
					if (!partyMembers.ContainsPlayer (user.username) && user.active) {
						UpdateService.GetInstance ().SendUpdate (new string[]{user.username}, UpdateService.CreateMessage (UpdateType.PartyRequest));
					}
					controller.Close ();
				}, (error) => {
					Debug.LogError (error);
				});
			});	
		}
	}

	private void AddPlayer (string username) {
		GameObject newPlayer = (GameObject) Instantiate (playerPrefab);
		newPlayer.transform.SetParent (transform);
		newPlayer.GetComponent<PartyEntry> ().ChangeName (username);
	}

	public void RequestLeaveParty () {
		DBServer.GetInstance ().LeaveParty (CurrentUser.GetInstance ().GetUserInfo ().username, () => {
			UpdateParty ();
		}, (error) => {
			Debug.LogError (error);
		});
	}

	private void UpdateParty () {
		if (CurrentUser.GetInstance ().GetUserInfo ().party.GetSize () == 0) {
			tabController.DestroyChat (partyChatName);
			partyChatName = null;
			NetworkService.GetInstance ().LeaveRoom ();
			menuController.SwtichToMenuView ();
		} else {
			ClearParty ();
			foreach (var member in CurrentUser.GetInstance ().GetUserInfo ().party.partyMembers) {
				AddPlayer (member);
			}
			if (partyChatName == null) {
				partyChatName = CurrentUser.GetInstance ().GetUserInfo ().party.owner;
				tabController.AddChat (partyChatName, false);
			}
		}
	}

	public void ClearParty () {
		foreach (var obj in GameObject.FindGameObjectsWithTag ("PlayerPartyEntity")) {
			Destroy (obj);
		}
	}

	public void Join () {
		menuController.SwitchToPartyView ();
		UpdateParty ();
	}

	public void JoinParty (string ownerParty) {
		if(!CurrentUser.GetInstance ().GetUserInfo ().party.ContainsPlayer(ownerParty)) {
			DBServer.GetInstance ().JoinParty (ownerParty, CurrentUser.GetInstance ().GetUserInfo ().username, () => {
				Join ();
			}, (error) => {
				Debug.LogError(error);
			});
		} else {
			Debug.Log("Duplicate invite");
		}
	}

	public void OnReceivedInvite (string from) {
		ConfirmAlertController.Create ("You have received a party invite from " + from, (alert) => {
			JoinParty (from);
			alert.Close ();
		}, (alert) => {
			alert.Close ();
		});
	}

	public void Update () {
		var owner = CurrentUser.GetInstance ().GetUserInfo ().party.owner;
		addPlayer.SetActive (owner == CurrentUser.GetInstance ().GetUserInfo ().username);
		gameModeLabel.text = menuController.GetIsAdventure () ?  "Game Mode\n--Adventure--" : "Game Mode\n--Endless--";
	}
}
