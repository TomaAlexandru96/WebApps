using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTab : MonoBehaviour {
	
	private String chName;
	private int chatNum;
	public GameObject chatTab;
	public GameObject closeButton;
	public bool isCloseable;

	public void Start () {
		closeButton.SetActive (isCloseable);
	}

	public void SelectChat () {
		getChatView ().GetComponent<ScrollRect> ().content = 
			(RectTransform)getChatView ().GetChild(0).transform.GetChild (chatNum+1);
		GameObject.FindGameObjectWithTag ("Chat").GetComponent<ChatController> ().LoadChat (chName);

		DeactivateButtons ();
		chatTab.GetComponent<Image> ().color = Color.green;
	}

	private Transform getChatView() {
		return GameObject.FindGameObjectWithTag ("Chat").transform.GetChild (0);
	} 

	private void DeactivateButtons() {
		for (int i = 0; i < GameObject.FindGameObjectWithTag ("ChatButtons").transform.childCount; i++) {
			GameObject.FindGameObjectWithTag ("ChatButtons").transform.GetChild(i).GetComponent<Image>().color = new Color32(255,255,225,240);
		}
	}

	public void CloseTab () {
		ChatService.GetInstance ().Unsubscribe (new string[]{chName});
		Destroy (gameObject);
	}

	public void UpdateName (String name, int num) {
		chName = name;
		chatNum = num;
		transform.GetChild (0).GetComponent<Text> ().text = name;
	}

	public string GetName () {
		return chName;
	}
}
