using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdventureController : MonoBehaviour {

	public GameObject loadingScreen;
	private HashSet<string> loadedPlayers;

	public void Awake () {
		loadedPlayers = new HashSet<string> ();
		loadedPlayers.Add (CurrentUser.GetInstance ().GetUserInfo ().username);

		UpdateService.GetInstance ().Subscribe (UpdateType.GameLoaded, (sender, message) => {
			loadedPlayers.Add (sender);
			if (AllPartyUsersLoaded ()) {
				StartGame ();
			}
		});
	}

	public void StartGame () {
		loadingScreen.SetActive (false);
	}

	public void Start () {
		GameObject.FindGameObjectWithTag ("Chat").GetComponent<ChatController> ().InitDefaultChat ();
		UpdateService.GetInstance ().SendUpdate (CurrentUser.GetInstance ().GetUserInfo ().party.partyMembers, 
					UpdateService.CreateMessage (UpdateType.GameLoaded));
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
