using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdventureController : Photon.MonoBehaviour {

	public GameObject loadingScreen;
	public GameObject party;
	public Transform[] playerSpawnPoints;
	public GameObject player;
	public GameObject[] enemies;

	private HashSet<string> loadedPlayers;

	public void Awake () {
		loadedPlayers = new HashSet<string> ();
	}

	public void OnApplicationQuit () {
		DBServer.GetInstance ().Logout (false, () => {
			
		}, (error) => {
			
		});
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
			StartCoroutine (Waves ());

			//SpawnEnemies ();
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
		NetworkService.GetInstance ().Spawn (player.name, 
			playerSpawnPoints [CurrentUser.GetInstance ().GetPositionInParty ()].position, Quaternion.identity, 0, new object[1] {CurrentUser.GetInstance ().GetUserInfo ()});
	}

	public IEnumerator Waves() {
		GameObject[] spawnersObj = GameObject.FindGameObjectsWithTag ("Spawner");
		Spawner[] spawners = new Spawner[spawnersObj.Length];
		for (int i = 0; i < spawnersObj.Length; i++) {
			spawners[i] = spawnersObj [i].GetComponent<Spawner> ();
		}

		for (int i = 0; i < spawnersObj.Length; i++) {
			spawners [i].Spawn (new string[] {"EnemyGit"}, new int[] {99}, 1, 2, 2f);
		}

		yield return new WaitForSeconds (30f);

		for (int i = 0; i < spawnersObj.Length; i++) {
			spawners [i].Spawn (new string[] {"EnemyHTML", "EnemyJS", "EnemyCSS"}, new int[] {33, 66, 99}, 3, 5, 1f);
		}
	}

	/*public void SpawnEnemies () {
		for (int i = 0; i < 10; i++) {
			NetworkService.GetInstance ().SpawnScene (enemies [0].name, new Vector3 (7.795f, -3f, 0f), Quaternion.identity, 0);	
		}
		NetworkService.GetInstance ().SpawnScene (enemies [0].name, new Vector3 (7.795f, -4f, 0f), Quaternion.identity, 0);	
	}*/
}
