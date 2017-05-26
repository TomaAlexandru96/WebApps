using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTab : MonoBehaviour {
	
	private String chName;

	public void SelectChat () {
		GameObject.FindGameObjectWithTag ("Chat").GetComponent<ChatController> ().LoadChat (chName);
	}

	public void UpdateName (String name) {
		chName = name;
		transform.GetChild (0).GetComponent<Text> ().text = name;
	}
}
