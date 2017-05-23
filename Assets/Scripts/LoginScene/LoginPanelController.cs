using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelController : MonoBehaviour {
	
	public InputField username;
	public InputField password;
	public GameObject mainPanel;

	/* Used by login button to issue a login request to the server */
	public void RequestLogin () {
		if (!CheckInput ()) {
			return;
		}
		User user = DBServer.Login (username.text, password.text);
		Debug.Log (user);
	}

	/* Checks validity of input and displays error message if any */
	public bool CheckInput () {
		return true;
	}

	/* Used to return to main scene */
	public void GoBack () {
		this.gameObject.SetActive (false);
		mainPanel.SetActive (true);
	}
}
