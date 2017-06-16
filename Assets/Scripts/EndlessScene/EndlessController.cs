using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessController : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject partyPrefab;
	public GameObject loadingScreen;
	public bool withAnimation;

	// Use this for initialization
	void Start () {
		GameObject.FindGameObjectWithTag ("DungeonGenerator").GetComponent<DungeonGenerator> ().BeginGeneration (withAnimation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnApplicationQuit () {
		CurrentUser.GetInstance ().UnsubscribeCH (CurrentUser.GetInstance ().GetUserInfo ().party.owner);
		DBServer.GetInstance ().LeaveParty (CurrentUser.GetInstance ().GetUserInfo ().username, () => {
			DBServer.GetInstance ().Logout (false, () => {}, (err) => {});
		}, (error) => {
			Debug.LogError (error);	
		});
	}

	public void SpawnPlayer (Vector3 position) {
		/*ChatController.GetChat ().InitDefaultChat ();*/
		GameObject player = NetworkService.GetInstance ().Spawn (playerPrefab.name, position, Quaternion.identity, 0,
			new object[1] {CurrentUser.GetInstance ().GetUserInfo ()});
		NetworkService.GetInstance ().SpawnScene (partyPrefab.name, Vector3.zero, Quaternion.identity, 0);
		loadingScreen.SetActive (false);
	}
}
