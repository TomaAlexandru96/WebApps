using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Text;

public class NetworkService : NetworkManager {

	public const String GAME_VERSION = "v0.01";
	public const String partyPrefabName = "Party";
	private static NetworkService instance = null;

	private MatchInfo info;

	private Action onFinish;

	public void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	public void StartService (Action onFinish) {
		this.onFinish = onFinish;
		StartServer ();
		StartMatchMaker ();
	}

	public void StopService () {
	}

	public override void OnStartServer () {
		onFinish ();
	}

	public static NetworkService GetInstance () {
		return instance;
	}

	public void GetRoomList (Action<List<MatchInfoSnapshot>> func) {
		matchMaker.ListMatches (0, 10, "", false, 0, 0, (success, extendedInfo, matches) => {
			func (matches);
		});
	}

	public void JoinLobby (int mode) {
		/*switch (mode) {
		case PartyMembers.STORY:
			selected = storyMaker;
			break;
		case PartyMembers.ADVENTURE:
			selected = adventureMaker;
			break;
		case PartyMembers.ENDLESS:
			selected = endlessMaker;
			break;
		}*/
	}

	public void JoinRoom (string roomName) {
		 // selected.JoinMatch;
	}

	public void CreateRoom (string roomName) {
		matchMaker.CreateMatch (roomName, 4, true, "", "", "", 0, 0, (success, extendedInfo, info) => {
			this.info = info;
		});
	}

	public void LeaveRoom () {
		matchMaker.DropConnection (info.networkId, info.nodeId, 0, (success, extendedInfo) => {
			this.info = null;
		});
	}

	public void LoadScene (int mode) {
		if (mode == 1) {
			ServerChangeScene ("Adventure");
		} else if (mode == 2) {
			ServerChangeScene ("Endless");
		} else {
			ServerChangeScene ("Story");
		}
	}

	public GameObject Spawn (string prefabName, Vector3 position, Quaternion rotation, int groupID) {
		return null;
	}

	public GameObject Spawn (string prefabName, Vector3 position, Quaternion rotation, int groupID, object[] data) {
		return null;
	}

	public void Destroy (GameObject ob) {
	}

	public GameObject SpawnScene (string prefabName, Vector3 position, Quaternion rotation, int groupID) {
		return null;
	}

	public bool IsMasterClient () {
		return false;
	}

	public void OnConnectedToPhoton () {
		onFinish ();
	}

	public bool IsInRoom () {
		return false;
	}
}
