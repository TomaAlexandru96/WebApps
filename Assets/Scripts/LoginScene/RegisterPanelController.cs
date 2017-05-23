using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanelController : MonoBehaviour {

	public InputField username;
	public InputField email;
	public InputField password;
	public InputField confirmPassword;
	public GameObject mainPanel;
	public Text errorLabel;

	/* Used by register button to issue aregister query to the DB */
	public void RequestRegister () {
		errorLabel.text = "";
		Response<User> response = DBServer.Register (username.text, password.text, email.text);

		if (response.error != "") {
			errorLabel.text = response.error;
		} else if (response.status != HttpStatusCode.OK) {
			errorLabel.text = response.status.ToString ();
		} else {
			Debug.Log("Registered: " + response.data.username);
		}
	}

	/* Used by cancel button to go back to the main pane */
	public void GoBack () {
		Activate (false);
		mainPanel.SetActive (true);
	}

	public void Activate (bool status) {
		errorLabel.text = "";
		gameObject.SetActive (status);
	}
}
