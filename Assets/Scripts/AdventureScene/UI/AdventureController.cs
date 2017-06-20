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
	public GameObject nameOfWavePanel;
	public GameObject infoPanel;

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
		nameOfWavePanel.transform.GetChild (0).GetComponent<Text> ().fontSize = 120;
		nameOfWavePanel.transform.GetChild (0).GetComponent<Text> ().fontStyle = UnityEngine.FontStyle.BoldAndItalic;
		infoPanel.SetActive (false);
	}

	[PunRPC]
	public void OnLoaded (string name) {
		loadedPlayers.Add (name);
		if (AllPartyUsersLoaded ()) {
			StartGame ();
		}
	}

	public IEnumerator DisplayWaveName (string waveName) {
		nameOfWavePanel.transform.GetChild (0).GetComponent<Text> ().text = waveName;
		nameOfWavePanel.SetActive (true);
		yield return new WaitForSeconds (2f);
		nameOfWavePanel.SetActive (false);

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

	public void ShowInfo () {
		infoPanel.SetActive (true);
	}


	public void HideInfo () {
		infoPanel.SetActive (false);
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

	
		StartCoroutine (DisplayWaveName ("Git Wave"));
		// GIT WAVE
		for (int i = 0; i < spawnersObj.Length; i++) {
			spawners [i].Spawn (new string[] {"EnemyGit"}, new int[] {99}, 1, 2, 2f);
		}

		yield return new WaitForSeconds (30f);

		StartCoroutine (DisplayWaveName ("Web Wave"));
		// WEB WAVE
		for (int i = 0; i < spawnersObj.Length; i++) {
			spawners [i].Spawn (new string[] {"EnemyHTML", "EnemyJS", "EnemyCSS"}, new int[] {33, 66, 99}, 3, 5, 1f);
		}

		yield return new WaitForSeconds (60f);

		StartCoroutine (DisplayWaveName ("Python Wave"));
		// PYTHON WAVE
		for (int i = 0; i < spawnersObj.Length; i++) {
			spawners [i].Spawn (new string[] {"EnemyPython"}, new int[] {99}, 1, 2, 1f);
		}

		yield return new WaitForSeconds (60f);
	}

	/*public void SpawnEnemies () {
		for (int i = 0; i < 10; i++) {
			NetworkService.GetInstance ().SpawnScene (enemies [0].name, new Vector3 (7.795f, -3f, 0f), Quaternion.identity, 0);	
		}
		NetworkService.GetInstance ().SpawnScene (enemies [0].name, new Vector3 (7.795f, -4f, 0f), Quaternion.identity, 0);	
	}*/
}
