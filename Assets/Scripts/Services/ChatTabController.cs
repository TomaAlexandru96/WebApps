using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTabController : MonoBehaviour {

	public GameObject chat;
	public GameObject party;

	private int maxChat = 6;
	private int totalChats = 1;

	public void addChat() {
		if (totalChats < maxChat) {
			GameObject newPlayer = Instantiate (chat, party.transform);
			Vector3 position = chat.GetComponent<RectTransform> ().localPosition;
			newPlayer.GetComponent<RectTransform> ().localPosition = new Vector3 (position.x + 80, position.y, position.z);


			chat = newPlayer;
			totalChats++;
		}
	} 
}
