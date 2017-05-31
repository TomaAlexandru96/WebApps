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
	private Action unsub1;
	private Action unsub2;
	private Action unsub3;
	private Action unsub4;
	private Action unsub5;

	public void Awake () {
		unsub1 = UpdateService.GetInstance ().Subscribe (UpdateType.PartyRequest, (sender, message) => {
			ConfirmAlertController.Create ("You have received a party invite from " + sender, (alert) => {
				owner = sender;
				UpdateService.GetInstance ().SendUpdate (new string[]{owner}, 
					UpdateService.CreateMessage (UpdateType.PartyRequestAccept));

				tabController.AddChat (owner, false);
				tabController.SetChat (CurrentUser.GetInstance ().GetUserInfo ().username, false);
				alert.Close ();
			}, (alert) => {
				alert.Close ();
			});
		});

		unsub2 = UpdateService.GetInstance ().Subscribe (UpdateType.PartyRequestAccept, (sender, message) => {
			partyMembers.AddPlayer (sender);
			message = UpdateService.CreateMessage (UpdateType.PartyUpdate, 
				UpdateService.CreateKV ("members", partyMembers));
			UpdateService.GetInstance ().SendUpdate (partyMembers.GetMembers (), message);
			UpdateParty();
		});

		unsub3 = UpdateService.GetInstance ().Subscribe (UpdateType.PartyUpdate, (sender, message) => {
			partyMembers = UpdateService.GetData<PartyMembers> (message, "members");

			UpdateParty();
		});

		unsub4 = UpdateService.GetInstance ().Subscribe (UpdateType.PartyLeft, (sender, message) => {
			String userLeft = UpdateService.GetData<String> (message, "user");
			partyMembers.RemovePlayer (userLeft);

			UpdateService.GetInstance ().SendUpdate (partyMembers.GetMembers (), 
						UpdateService.CreateMessage (UpdateType.PartyUpdate, UpdateService.CreateKV ("members", partyMembers)));

			UpdateParty();
		});

		unsub5 = UpdateService.GetInstance ().Subscribe (UpdateType.PartyDisbaned, (sender, message) => {
			CurrentUser.GetInstance ().RequestUpdate ((user) => {
				owner = CurrentUser.GetInstance ().GetUserInfo ().username;
				partyMembers.RemoveAllButOwner (owner);
				UpdateParty ();
			});
		});
	}

	public void Start () {
		owner = CurrentUser.GetInstance ().GetUserInfo ().username;
		partyMembers.AddPlayer (owner);
		AddPlayer (CurrentUser.GetInstance ().GetUserInfo ().username);
		CurrentUser.GetInstance ().SetParty (this);
	}

	public void OnDestroy () {
		unsub1 ();
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
		Debug.Log (owner);
		tabController.SetChat (CurrentUser.GetInstance ().GetUserInfo ().username, false);
		tabController.SetChat (owner, true);
	}

	public void AddPlayer(string username) {
		GameObject newPlayer = Instantiate (playerPrefab);
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
}
