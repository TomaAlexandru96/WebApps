using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomName : MonoBehaviour {

	public GameObject roomName; 

	public void UpdateRoomName(string name) {
		roomName.GetComponent<Text> ().text = name;
	}
}
