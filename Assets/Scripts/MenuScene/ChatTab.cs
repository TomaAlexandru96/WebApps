using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTab : MonoBehaviour {
	
	private String chName;
	public GameObject chatTab; 

	public void SelectChat () {
		GameObject.FindGameObjectWithTag ("Chat").GetComponent<ChatController> ().LoadChat (chName);
		DeactivateButtons ();
		chatTab.GetComponent<Image> ().color = new Color32 (0, 0, 0, 0);
	}

	private void DeactivateButtons() {
		for (int i = 0; i < GameObject.FindGameObjectWithTag ("ChatButtons").transform.childCount; i++) {
			GameObject.FindGameObjectWithTag ("ChatButtons").transform.GetChild(i).GetComponent<Image>().color = new Color32(255,255,225,240);
		}
	}

	public void UpdateName (String name) {
		chName = name;
		transform.GetChild (0).GetComponent<Text> ().text = name;
	}
}
