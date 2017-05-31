using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsEntry : MonoBehaviour {

	public GameObject friendsPanel;
	private GameObject optionPanel;

	public void Start() {
		optionPanel = GameObject.FindGameObjectWithTag ("Menu").transform.GetChild(3).gameObject;
		optionPanel.SetActive (false);
		UpdateStatus ();

		UpdateService.GetInstance ().Subscribe (UpdateType.UserUpdate, (sender, message) => {
			if (GetName ().Equals (sender)) {
				UpdateStatus ();
			}
		});
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

	private void UpdateStatus () {
		DBServer.GetInstance ().FindUser (GetName (), (user) => {
			ColorBlock cb = gameObject.GetComponent<Button> ().colors;
			cb.normalColor = (user.active ? Color.green : Color.red);
			gameObject.GetComponent<Button> ().colors = cb;
		}, (error) => {
			Debug.LogError (error);
		});
	}

	public void SetName (string name) {
		gameObject.GetComponentInChildren<Text> ().text = name;
	}

	public string GetName () {
		return gameObject.GetComponentInChildren<Text> ().text;
	}
}
