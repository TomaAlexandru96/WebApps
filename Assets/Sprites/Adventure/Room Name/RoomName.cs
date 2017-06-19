using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomName : MonoBehaviour {

	public GameObject roomName; 

	public void UpdateRoomName(string name) {
		roomName.GetComponent<Text> ().text = name;

		if (name.Length > 20) {
			roomName.GetComponent<Text> ().fontSize = 11;
		} else {
			roomName.GetComponent<Text> ().fontSize = 14;
		}
	}
}
