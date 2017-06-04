using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePartyController : MonoBehaviour {
	public Text ownerName;
	public Text size;
	public Button joinBtn;
	private string roomName;
	private int roomPlayers = 0;
	private int mode;

	public void Start () {
		ownerName.text = roomName;
		size.text = roomPlayers.ToString () + "/4";
		joinBtn.interactable = roomPlayers < 4;
	}

	public void SetRoomStats (RoomInfo info, int mode) {
		this.roomName = info.Name;
		this.roomPlayers = info.PlayerCount;
		this.mode = mode;
	}

	public void Join () {
		GameObject.FindGameObjectWithTag ("Menu").GetComponent <MenuController> ().SwitchToPartyView ();
		GameObject.FindGameObjectWithTag ("Party").GetComponent <Party> ().JoinParty (roomName, mode);
	}
}
