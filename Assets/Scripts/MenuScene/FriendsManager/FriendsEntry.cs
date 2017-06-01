using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsEntry : MonoBehaviour {

	public GameObject friendsPanel;
	public Image avatar;
	private GameObject optionPanel;
	private Action unsub1;
	private Action unsub2;
	private Action unsub3;

	public void Awake () {
		unsub1 = UpdateService.GetInstance ().Subscribe (UpdateType.LoginUser, (sender, message) => {
			if (GetName ().Equals (sender)) {
				ChangeStatus (true);
			}
		});

		unsub2 = UpdateService.GetInstance ().Subscribe (UpdateType.LogoutUser, (sender, message) => {
			if (GetName ().Equals (sender)) {
				ChangeStatus (false);
			}
		});

		unsub3 = UpdateService.GetInstance ().Subscribe (UpdateType.UserUpdate, (sender, message) => {
			if (GetName ().Equals (sender)) {
				UpdateStatus (1f);
			}
		});
	}

	public void Start() {
		optionPanel = GameObject.FindGameObjectWithTag ("Menu").transform.GetChild(3).gameObject;
		optionPanel.SetActive (false);
		UpdateStatus (0f);
	}

	public void OnDestroy () {
		unsub1 ();
		unsub2 ();
		unsub3 ();
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

	private void ChangeStatus (bool status) {
		ColorBlock cb = gameObject.GetComponent<Button> ().colors;
		cb.normalColor = (status ? Color.green : Color.red);
		gameObject.GetComponent<Button> ().colors = cb;
	}

	private void UpdateStatus (float delay) {
		UpdateService.GetInstance ().Wait (delay);
		DBServer.GetInstance ().FindUser (GetName (), (user) => {
			Debug.Log (user);
			ChangeStatus (user.active);
			avatar.sprite = user.character.GetImage ();
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
