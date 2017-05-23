using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelController : MonoBehaviour {
	
	public InputField username;
	public InputField password;
	public GameObject mainPanel;
	public Text errorLabel;

	/* Used by login button to issue a login request to the server */
	public void RequestLogin () {
		if (!CheckInput ()) {
			return;
		}

		errorLabel.text = "";
		Response<User> response = DBServer.Login (username.text, password.text);

		if (response.error != "") {
			errorLabel.text = response.error;
		} else if (response.status != HttpStatusCode.OK) {
			errorLabel.text = response.status.ToString ();
		} else {
			Debug.Log("Logged: " + response.data.username);
		}
	}

	/* Checks validity of input and displays error message if any */
	public bool CheckInput () {
		return true;
	}

	/* Used to return to main scene */
	public void GoBack () {
		Activate (false);
		mainPanel.SetActive (true);
	}

	public void Activate (bool status) {
		errorLabel.text = "";
		gameObject.SetActive (status);
	}
}
