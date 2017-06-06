using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdventureController : Photon.PunBehaviour {

	public GameObject loadingScreen;
	public GameObject party;
	public Transform[] spawnPoints;
	public GameObject[] players;

	private HashSet<string> loadedPlayers;

	public void Awake () {
		loadedPlayers = new HashSet<string> ();
	}

	public void OnApplicationQuit () {
		ExitGame ();
	}

	public void Start () {
		GameObject.FindGameObjectWithTag ("Chat").GetComponent<ChatController> ().InitDefaultChat ();
		SpawnPlayer ();
		ChatController.GetChat ().withFadeOut = true;
		photonView.RPC ("OnLoaded", PhotonTargets.All, CurrentUser.GetInstance ().GetUserInfo ().username);
	}

	[PunRPC]
	public void OnLoaded (string name) {
		loadedPlayers.Add (name);
		if (AllPartyUsersLoaded ()) {
			StartGame ();
		}
	}

	public void StartGame () {
		if (NetworkService.GetInstance ().IsMasterClient ()) {
			NetworkService.GetInstance ().SpawnScene (party.name, Vector3.zero, Quaternion.identity, 0);
		}
		loadingScreen.SetActive (false);
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

	public void SpawnPlayer () {
		// to be changed
		NetworkService.GetInstance ().Spawn (players [CurrentUser.GetInstance ().GetUserInfo ().character.type].name, 
			spawnPoints [CurrentUser.GetInstance ().GetPositionInParty ()].position, Quaternion.identity, 0, new object[1] {CurrentUser.GetInstance ().GetUserInfo ().username});
	}
}
