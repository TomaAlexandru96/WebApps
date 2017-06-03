using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseAvatarPanelController : MonoBehaviour {

	public GameObject registerPanel;
	public List<GameObject> avatars = new List<GameObject> ();
	public int characterNumber;
	public InputField characterName;

	public void RequestCharacter () {
		DBServer.GetInstance ().ChooseCharacter (CurrentUser.GetInstance ().GetUserInfo (), characterName.text, characterNumber, () => {
			SceneManager.LoadScene ("Menu");
		}, (errorCode) => {
			String errorMessage = errorCode + ": ";
			switch (errorCode) {
			case DBServer.NOT_FOUND_STATUS: errorMessage += "Username or password combination wrong!\n";break;
			default: errorMessage += "Could not connect to the server!\n";break;
			}

		}); 
		
	}

	public void AvatarChosen(int num) {
		foreach (GameObject avatar in avatars) {
			avatar.transform.GetComponent<Image> ().color = new Color32 (200, 200, 200, 100);
		}
		Debug.Log ("Avatar " + num);
		avatars [num].transform.GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);
	}
}
