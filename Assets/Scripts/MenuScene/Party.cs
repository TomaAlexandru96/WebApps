using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour {

	public const int maxSize = 4;

	public GameObject playerPrefab;
	public GameObject addPlayer;
	public GameObject leaveParty;
	public ChatTabController tabController;

	private PartyMembers partyMembers = new PartyMembers ();
	private string owner;
	private Action unsub2;
	private Action unsub3;
	private Action unsub4;
	private Action unsub5;

	public void Awake () {
		unsub2 = UpdateService.GetInstance ().Subscribe (UpdateType.PartyRequestAccept, (sender, message) => {
			OnPartyAccept (sender);
		});

		unsub3 = UpdateService.GetInstance ().Subscribe (UpdateType.PartyUpdate, (sender, message) => {
			OnPartyUpdate (UpdateService.GetData<PartyMembers> (message, "members"));
		});

		unsub4 = UpdateService.GetInstance ().Subscribe (UpdateType.PartyLeft, (sender, message) => {
			OnPartyMemberLeft (UpdateService.GetData<String> (message, "user"));
		});

		unsub5 = UpdateService.GetInstance ().Subscribe (UpdateType.PartyDisbaned, (sender, message) => {
			DisbandParty ();
		});
	}

	public void OnReceivedInvite (string from) {
		ConfirmAlertController.Create ("You have received a party invite from " + from, (alert) => {
			owner = from;
			if(!partyMembers.ContainsPlayer(from)){
				UpdateService.GetInstance ().SendUpdate (new string[]{owner}, 
					UpdateService.CreateMessage (UpdateType.PartyRequestAccept));

				tabController.AddChat (owner, false);
				tabController.SetChat (CurrentUser.GetInstance ().GetUserInfo ().username, false);

				MoveToPartyPanel ();

			} else {
				Debug.Log("Duplicate invite");
			}
			alert.Close ();
		}, (alert) => {
			alert.Close ();
		});
	}

	public void MoveToPartyPanel () {
		transform.parent.gameObject.SetActive (true);
	}

	public void MoveToMenuPanel () {
		gameObject.SetActive (false);
	}

	public void OnPartyAccept (string from) {
		partyMembers.AddPlayer (from);
		var message = UpdateService.CreateMessage (UpdateType.PartyUpdate, 
							UpdateService.CreateKV ("members", partyMembers));
		UpdateService.GetInstance ().SendUpdate (partyMembers.GetMembers (), message);
		UpdateParty();
	}

	public void OnPartyUpdate (PartyMembers newPartyMembers) {
		this.partyMembers = newPartyMembers;
		UpdateParty();
	}

	public void OnPartyMemberLeft (string userLeft) {
		partyMembers.RemovePlayer (userLeft);

		UpdateService.GetInstance ().SendUpdate (partyMembers.GetMembers (), 
			UpdateService.CreateMessage (UpdateType.PartyUpdate, UpdateService.CreateKV ("members", partyMembers)));

		UpdateParty();
	}

	public void DisbandParty () {
		CurrentUser.GetInstance ().RequestUpdate ((user) => {
			owner = CurrentUser.GetInstance ().GetUserInfo ().username;
			partyMembers.RemoveAllButOwner (owner);
			UpdateParty ();
		});
	}

	public void Start () {
		owner = CurrentUser.GetInstance ().GetUserInfo ().username;
		partyMembers.AddPlayer (owner);
		AddPlayer (CurrentUser.GetInstance ().GetUserInfo ().username);
		CurrentUser.GetInstance ().SetParty (this);
	}

	public void OnDestroy () {
		unsub2 ();
		unsub3 ();
		unsub4 ();
		unsub5 ();
	}

	public void RequestAddPlayer () {
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

	private void UpdateParty () {
		ClearParty ();
		foreach (var member in partyMembers.GetMembers ()) {
			AddPlayer (member);
		}

		tabController.SetChat (CurrentUser.GetInstance ().GetUserInfo ().username, false);
		tabController.SetChat (owner, true);
	}

	public void AddPlayer(string username) {
		GameObject newPlayer = (GameObject) Instantiate (playerPrefab);
		newPlayer.transform.SetParent (transform);
		newPlayer.GetComponent<PartyEntry> ().ChangeName (username);
	}

	public void RequestLeaveParty () {
		if (owner == CurrentUser.GetInstance ().GetUserInfo ().username) {
			// disband party
			UpdateService.GetInstance ().SendUpdate (partyMembers.GetMembers (), UpdateService.CreateMessage (UpdateType.PartyDisbaned));
		}
		UpdateService.GetInstance ().SendUpdate (new string[]{owner}, UpdateService.CreateMessage (UpdateType.PartyLeft, 
					UpdateService.CreateKV ("user", CurrentUser.GetInstance ().GetUserInfo ().username)));
		owner = CurrentUser.GetInstance ().GetUserInfo ().username;
		partyMembers.RemoveAllButOwner (owner);
		UpdateParty ();
	}

	public void ClearParty () {
		foreach (var obj in GameObject.FindGameObjectsWithTag ("PlayerPartyEntity")) {
			Destroy (obj);
		}
	}

	public void Update () {
		if (addPlayer == null || leaveParty == null) {
			return;
		}
		addPlayer.SetActive (owner == CurrentUser.GetInstance ().GetUserInfo ().username);
		leaveParty.SetActive (partyMembers.GetSize () > 1);
	}

	public PartyMembers getPartyMembers () {
		return partyMembers;
	}
}
