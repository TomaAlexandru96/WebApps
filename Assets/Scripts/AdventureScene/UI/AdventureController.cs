using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdventureController : Photon.MonoBehaviour {

	public GameObject loadingScreen;
	public GameObject party;

	private HashSet<string> loadedPlayers;

	public void Awake () {
		loadedPlayers = new HashSet<string> ();
		loadedPlayers.Add (CurrentUser.GetInstance ().GetUserInfo ().username);
	}

	public void StartGame () {
		loadingScreen.SetActive (false);
		if (NetworkService.GetInstance ().IsMasterClient ()) {
			GameObject partyPanel = NetworkService.GetInstance ().Spawn (party.name, Vector3.zero, Quaternion.identity, 0);
		}
	}

	public void OnApplicationQuit () {
		ExitGame ();
	}

	public void Start () {
		GameObject.FindGameObjectWithTag ("Chat").GetComponent<ChatController> ().InitDefaultChat ();
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

	public void OnPhotonSerialiseView (PhotonStream stream, PhotonMessageInfo info) {
		Debug.Log (info);
		if (stream.isWriting) {
			Debug.Log ("writting");
		} else {
			Debug.Log ("reading");
		}
	}
}
