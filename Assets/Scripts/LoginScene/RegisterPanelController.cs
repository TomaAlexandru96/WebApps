using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterPanelController : MonoBehaviour {

	public InputField username;
	public InputField email;
	public InputField password;
	public InputField confirmPassword;
	public GameObject mainPanel;
	public Text errorLabel;

	/* Used by register button to issue aregister query to the DB */
	public void RequestRegister () {
		if (!CheckInput ()) {
			return;
		}

		Response<User> response = DBServer.Register (username.text, password.text, email.text);

		if (response.error != null) {
			if (response.error.Status == WebExceptionStatus.ConnectFailure) {
				errorLabel.text = "Could not connect to the server!\n";
			} else if (response.error.Status == WebExceptionStatus.ReceiveFailure) {
				errorLabel.text = "Username already exists\n";
			} else {
				errorLabel.text = "Error unknown!";
			}
		} else {
			CurrentUser.GetInstance ().SetUserInfo (response.data);
			SceneManager.LoadScene ("Menu");
		}
	}

	/* Checks validity of input and displays error message if any */
	public bool CheckInput () {
		errorLabel.text = "";
		errorLabel.text += Validator.isUsernameValid (username.text);
		errorLabel.text += Validator.isEmailValid (email.text);
		errorLabel.text += Validator.isPasswordValid (password.text);

		if (!errorLabel.text.Equals ("")) {
			return false;
		}

		if (!password.text.Equals (confirmPassword.text)) {
			errorLabel.text += "Passwords do not match!\n";
		}

		return errorLabel.text.Equals ("");
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
