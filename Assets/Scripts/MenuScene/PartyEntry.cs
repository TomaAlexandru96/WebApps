using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyEntry : MonoBehaviour {

	public Text usernameText;

	public void ChangeName (string username) {
		usernameText.text = username;
	}
}
