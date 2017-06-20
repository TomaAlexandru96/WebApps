using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AvatarTypePanel : MonoBehaviour {

	public List<GameObject> avatars = new List<GameObject> ();
	public int characterNumber;

	public GameObject avatarTypePanel;
	public GameObject chooseAvatarPanel;

	private string characterName;
	private int characterID;

	public void RequestCharacter () {
		DBServer.GetInstance ().ChooseCharacter (CurrentUser.GetInstance ().GetUserInfo (), characterName, (PlayerType) characterNumber, characterID, () => {
			SceneManager.LoadScene ("Menu");
		}, (errorCode) => {
			string errorMessage = errorCode + ": ";
			switch (errorCode) {
			case DBServer.NOT_FOUND_STATUS: errorMessage += "Username or password combination wrong!\n";break;
			default: errorMessage += "Could not connect to the server!\n";break;
			}
		});
	}

	public void SetCHName (string characterName) {
		this.characterName = characterName;
	}

	public void SetCHID (int characterID) {
		this.characterID = characterID;
	}

	public void Back () {
		avatarTypePanel.SetActive (false);
		chooseAvatarPanel.SetActive (true);
	}

	public void AvatarChosen(int num) {
		foreach (GameObject avatar in avatars) {
			avatar.transform.GetComponent<Image> ().color = new Color32 (200, 200, 200, 100);
		}
		avatars [num].transform.GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);
	}
}
