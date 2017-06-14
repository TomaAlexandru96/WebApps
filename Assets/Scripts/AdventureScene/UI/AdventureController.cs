using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class AdventureController : NetworkBehaviour {

	public GameObject loadingScreen;
	public GameObject party;
	public GameObject player;
	public GameObject[] enemies;

	private SyncListString loadedPlayers = new SyncListString ();

	public void OnApplicationQuit () {
		DBServer.GetInstance ().Logout (false, () => {
		}, (error) => {
		});
		ExitGame ();
	}

	public void Start () {
		GameObject.FindGameObjectWithTag ("Chat").GetComponent<ChatController> ().InitDefaultChat ();
		CmdSpawnPlayer ();
		ChatController.GetChat ().withFadeOut = true;
		CmdOnLoaded (CurrentUser.GetInstance ().GetUserInfo ().username);
		Debug.Log (isServer);
		Debug.Log (isClient);
	}

	[Command]
	public void CmdSpawnPlayer () {
		ClientScene.AddPlayer ((short) CurrentUser.GetInstance ().GetPositionInParty ());
	}

	[Command]
	public void CmdOnLoaded (string name) {
		Debug.Log ("Scas");
		loadedPlayers.Add (name);
		if (AllPartyUsersLoaded ()) {
			RpcStartGame ();
		}
		Debug.Log (loadedPlayers.Count);
		Debug.Log ("Anything");
	}

	[ClientRpc]
	public void RpcStartGame () {
		if (isServer) {
			NetworkService.GetInstance ().Spawn (party, Vector3.zero, Quaternion.identity);
			CmdSpawnEnemies ();
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

	[Command]
	public void CmdSpawnEnemies () {
		for (int i = 0; i < 10; i++) {
			NetworkService.GetInstance ().Spawn (enemies [0], new Vector3 (7.795f, -3f, 0f), Quaternion.identity);	
		}
		NetworkService.GetInstance ().Spawn (enemies [0], new Vector3 (7.795f, -4f, 0f), Quaternion.identity);	
	}
}
