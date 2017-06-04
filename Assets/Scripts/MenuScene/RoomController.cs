using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviour {

	public MenuController menu;
	public GameObject content;
	public GameObject gamePartyPrefab;
	public Text modeName;

	public void Update () {
		modeName.text = menu.GetIsAdventure () ? "Adventure" : "Endless";
	}

	public void Refresh () {
		if (menu.GetIsAdventure ()) {
			foreach (var room in NetworkService.GetInstance ().GetAdventureRooms ()) {
				GameObject go = Instantiate (gamePartyPrefab);
				go.GetComponent <GamePartyController> ().SetRoomStats (room);
				go.transform.SetParent (content.transform);
			}
		}
	}
}
