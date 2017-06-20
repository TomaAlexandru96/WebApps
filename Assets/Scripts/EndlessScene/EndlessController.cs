using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.SceneManagement;

public class EndlessController : Photon.PunBehaviour {

	public GameObject playerPrefab;
	public GameObject partyPrefab;
	public GameObject loadingScreen;
	public GameObject canvas;
	public float lastAttack;

	public List<Player> players = new List<Player> ();

	// Use this for initialization
	void Start () {
		if (NetworkService.GetInstance ().IsMasterClient ()) {
			GameObject.FindGameObjectWithTag ("DungeonGenerator").GetComponent<DungeonGenerator> ().BeginGeneration (PlayerPrefs.GetInt ("endless_animation") != 0);
			lastAttack = Time.time;	
		}
	}

	void OnApplicationQuit () {
		CurrentUser.GetInstance ().UnsubscribeCH (CurrentUser.GetInstance ().GetUserInfo ().party.owner);
		DBServer.GetInstance ().LeaveParty (CurrentUser.GetInstance ().GetUserInfo ().username, () => {
			DBServer.GetInstance ().Logout (false, () => {}, (err) => {});
		}, (error) => {
			Debug.LogError (error);	
		});
	}

	void Update () {
		if (!NetworkService.GetInstance ().IsMasterClient ()) {
			return;
		}

		return;
		if (lastAttack + 0.2f >= Time.time) {
			return;
		}

		lastAttack = Time.time;

		foreach (var player in players) {
			Collider2D[] colls = new Collider2D[100];
			int len = player.gameObject.GetComponent<BoxCollider2D> ().GetContacts (colls);
			bool onLava = true;

			for (int i = 0; i < len; i++) {
				if (colls[i].tag.Equals ("NormalMapComponent")) {
					onLava = false;
				}
			}

			if (onLava) {
				player.DecreaseHealth (0.2f);
			}
		}
	}

	public void Exit () {
		CurrentUser.GetInstance ().UnsubscribeCH (CurrentUser.GetInstance ().GetUserInfo ().party.owner);
		DBServer.GetInstance ().LeaveParty (CurrentUser.GetInstance ().GetUserInfo ().username, () => {
			SceneManager.LoadScene ("Menu");
		}, (error) => {
			Debug.LogError (error);	
		});
	}

	public void OnFinishedDungeon (Vector3 position) {
		photonView.RPC ("SpawnPlayer", PhotonTargets.All, position);
	}

	[PunRPC]
	public void SpawnPlayer (Vector3 position) {
		canvas.SetActive (true);
		ChatController.GetChat ().InitDefaultChat ();
		GameObject player = NetworkService.GetInstance ().Spawn (playerPrefab.name, position, Quaternion.identity, 0,
			new object[1] {CurrentUser.GetInstance ().GetUserInfo ()});

		if (NetworkService.GetInstance ().IsMasterClient ()) {
			NetworkService.GetInstance ().SpawnScene (partyPrefab.name, Vector3.zero, Quaternion.identity, 0);
			players.Add (player.GetComponent<Player> ());
		}

		loadingScreen.SetActive (false);
	}
}
