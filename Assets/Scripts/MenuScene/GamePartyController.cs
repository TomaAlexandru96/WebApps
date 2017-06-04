using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePartyController : MonoBehaviour {
	public Text ownerName;
	public Text size;
	private string roomName;
	private int roomPlayers = 0;

	public void Start () {
		ownerName.text = roomName;
		size.text = roomPlayers.ToString () + "/4";
	}

	public void SetRoomStats (RoomInfo info) {
		this.roomName = info.Name;
		this.roomPlayers = info.PlayerCount;
	}

	public void Join () {
		GameObject.FindGameObjectWithTag ("Menu").GetComponent <MenuController> ().SwitchToPartyView ();
		GameObject.FindGameObjectWithTag ("Party").GetComponent <Party> ().JoinParty (roomName);
	}
}
