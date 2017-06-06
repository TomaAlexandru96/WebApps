using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdventureController : Photon.PunBehaviour {

	public GameObject loadingScreen;
	public GameObject party;

	private HashSet<string> loadedPlayers;

	public void Awake () {
		loadedPlayers = new HashSet<string> ();
	}

	public void StartGame () {
		loadingScreen.SetActive (false);
		if (NetworkService.GetInstance ().IsMasterClient ()) {
			NetworkService.GetInstance ().Spawn (party.name, Vector3.zero, Quaternion.identity, 0);
		}
	}

	public void OnApplicationQuit () {
		ExitGame ();
	}

	public void Start () {
		GameObject.FindGameObjectWithTag ("Chat").GetComponent<ChatController> ().InitDefaultChat ();
		photonView.RPC ("OnLoaded", PhotonTargets.All, CurrentUser.GetInstance ().GetUserInfo ().username);
	}

	[PunRPC]
	public void OnLoaded (string name) {
		loadedPlayers.Add (name);
		if (AllPartyUsersLoaded ()) {
			StartGame ();
		}
	}

	public bool AllPartyUsersLoaded () {
		foreach (var user in CurrentUser.GetInstance ().GetUserInfo ().party.partyMembers) {
			if (!loadedPlayers.Contains (user)) {
				return false;
			}
		}
		return true;
	}

	public void ExitGame () {
		CurrentUser.GetInstance ().UnsubscribeCH (CurrentUser.GetInstance ().GetUserInfo ().party.owner);
		DBServer.GetInstance ().LeaveParty (CurrentUser.GetInstance ().GetUserInfo ().username, () => {
			SceneManager.LoadScene ("Menu");
		}, (error) => {
			Debug.LogError (error);	
		});
	}
}
