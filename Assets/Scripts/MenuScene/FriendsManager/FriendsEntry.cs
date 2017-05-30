using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsEntry : MonoBehaviour {

	public GameObject friendsPanel;
	private GameObject optionPanel;

	public void Start() {
		optionPanel = GameObject.FindGameObjectWithTag ("Menu").transform.GetChild(5).gameObject;
		optionPanel.SetActive (false);
	}

	public void ShowOptions() {
		if (optionPanel.GetActive ()) { 
			optionPanel.SetActive (false);
		} else {
			optionPanel.SetActive (true);
			Vector3 friendPos = friendsPanel.transform.position;
			Vector3 OptionPos = optionPanel.transform.position;
			optionPanel.transform.position = new Vector3 (OptionPos.x, friendPos.y-30, OptionPos.z);
		}
	}
}
