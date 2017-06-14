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
		this.logLevel = LogFilter.FilterLevel.Error;
		StartMatchMaker ();
		onFinish ();
	}

	public void StopService () {
		StopMatchMaker ();
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

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
		GameObject player = Instantiate(playerPrefab);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		NetworkServer.SetClientReady (conn);
		//player.GetComponent <Player> ().SetUser (CurrentUser.GetInstance ().GetUserInfo ());
	}

	public void JoinRoom (string roomName) {
		matchMaker.ListMatches (0, 10, roomName, false, 0, 0, (success, extendedInfo, matches) => {
			matchMaker.JoinMatch (matches[0].networkId, "", "", "", 0, 0, (succ, extInfo, info) => {
				this.info = info;
				StartClient (info);
			});
		});
	}

	public void CreateRoom (string roomName) {
		matchMaker.CreateMatch (roomName, 4, true, "", "", "", 0, 0, (success, extendedInfo, info) => {
			this.info = info;
			StartHost (info);
		});
	}

	public void LeaveRoom () {
		if (info == null) {
			return;
		}

		matchMaker.DropConnection (info.networkId, info.nodeId, 0, (success, extendedInfo) => {
			StopHost ();
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

	public GameObject Spawn (GameObject prefab, Vector3 position, Quaternion rotation) {
		GameObject obj = Instantiate (prefab, position, rotation);
		NetworkServer.Spawn (obj);
		return obj;
	}

	public void Destroy (GameObject ob) {
		NetworkServer.Destroy (ob);
	}

	public bool IsInRoom () {
		return this.info != null;
	}
}
