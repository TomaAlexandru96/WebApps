using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour {

	public GameObject[] playerPrefabs;
	public Transform spawnPoint;

	// Use this for initialization
	void Start () {
		ChatController.GetChat ().InitDefaultChat ();
		NetworkService.GetInstance ().Spawn (playerPrefabs [CurrentUser.GetInstance ().GetUserInfo ().character.type].name, spawnPoint.position, Quaternion.identity, 0,
			new object[1] {CurrentUser.GetInstance ().GetUserInfo ().username});
	}

	void OnApplicationQuit () {
		CurrentUser.GetInstance ().UnsubscribeCH (CurrentUser.GetInstance ().GetUserInfo ().party.owner);
		DBServer.GetInstance ().LeaveParty (CurrentUser.GetInstance ().GetUserInfo ().username, () => {
			DBServer.GetInstance ().Logout (false, () => {}, (err) => {});
		}, (error) => {
			Debug.LogError (error);	
		});
	}

	// Update is called once per frame
	void Update () {
		
	}
}
