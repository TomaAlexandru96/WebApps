using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyControl : MonoBehaviour {

	public const int maxSize = 4;

	public GameObject playerPrefab;
	public GameObject addPlayer;

	private string owner;
	private List<string> partyMembers = new List<string> ();

	public void Start () {
		owner = CurrentUser.GetInstance ().GetUserInfo ().username;
		partyMembers.Add (owner);
		AddPlayer (CurrentUser.GetInstance ().GetUserInfo ().username);

		UpdateService.GetInstance ().Subscribe (UpdateType.PartyRequest, (sender, message) => {
			ConfirmAlertController.Create ("You have received a party invite from " + sender, (alert) => {
				owner = sender;
				UpdateService.GetInstance ().SendUpdate (new string[]{owner}, 
					UpdateService.CreateMessage (UpdateType.PartyRequestAccept));
				alert.Close ();
			}, (alert) => {
				alert.Close ();
			});
		});

		UpdateService.GetInstance ().Subscribe (UpdateType.PartyRequestAccept, (sender, message) => {
			partyMembers.Add (sender);
			UpdateService.GetInstance ().SendUpdate (partyMembers.ToArray (), 
				UpdateService.CreateMessage (UpdateType.PartyUpdate,
					UpdateService.CreateKV ("members", partyMembers)));
		});

		UpdateService.GetInstance ().Subscribe (UpdateType.PartyUpdate, (sender, message) => {
			partyMembers = UpdateService.GetData<List<string>> (message, "members");
			ClearParty ();
			foreach (var member in partyMembers) {
				AddPlayer (member);
			}
		});
	}

	private void RequestAddPlayer () {
		if (owner == CurrentUser.GetInstance ().GetUserInfo ().username && partyMembers.Count < maxSize) {
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
		GameObject newPlayer = Instantiate (playerPrefab);
		newPlayer.transform.SetParent (transform);
		newPlayer.GetComponent<PartyEntry> ().ChangeName (username);
	}

	public void ClearParty () {
		foreach (var obj in GameObject.FindGameObjectsWithTag ("PlayerParyEntity")) {
			DestroyImmediate (obj);
		}
	}

	public void Update () {
		addPlayer.SetActive (owner == CurrentUser.GetInstance ().GetUserInfo ().username);
	}
}
