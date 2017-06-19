using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseAvatarPanelController : MonoBehaviour {

	public GameObject registerPanel;
	public GameObject avatarTypePanel;
	public GameObject chooseAvatarPanel; 
	public List<GameObject> avatars = new List<GameObject> ();
	public int characterNumber;
	public InputField characterName;

	public void RequestCharacter () {
		avatarTypePanel.GetComponent<AvatarTypePanel> ().SetCHName (characterName.text);
		avatarTypePanel.GetComponent<AvatarTypePanel> ().SetCHID (characterNumber);
		chooseAvatarPanel.SetActive(false);
		avatarTypePanel.SetActive(true);
	}

	public void AvatarChosen (int num) {
		foreach (GameObject avatar in avatars) {
			avatar.transform.GetComponent<Image> ().color = new Color32 (200, 200, 200, 100);
		}
		avatars [num].transform.GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);
	}
}
