using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTab : MonoBehaviour {
	
	private String chName;
	public GameObject closeButton;
	public bool isCloseable;

	public void Start () {
		closeButton.SetActive (isCloseable);
	}

	public void SelectChat () {
		DeactivateButtons ();
		ChatController.GetChat ().LoadChat (chName);
		GetComponent<Image> ().color = Color.green;
	}

	private void DeactivateButtons() {
		for (int i = 0; i < GameObject.FindGameObjectWithTag ("ChatButtons").transform.childCount; i++) {
			GameObject.FindGameObjectWithTag ("ChatButtons").transform.GetChild(i).GetComponent<Image>().color = new Color32(255,255,225,240);
		}
	}

	public void CloseTab () {
		ChatController.GetChat ().DestroyChat (chName);
	}

	public void UpdateName (String name, String viewName) {
		chName = name;
		transform.GetChild (0).GetComponent<Text> ().text = viewName;
	}

	public string GetName () {
		return chName;
	}
}
