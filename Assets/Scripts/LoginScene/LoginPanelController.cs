using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

		DBServer.GetInstance ().Login (username.text, password.text, true, (user) => {
			SceneManager.LoadScene ("Menu");	
		}, (errorCode) => {
			String errorMessage = errorCode + ": ";
			switch (errorCode) {
			case DBServer.NOT_FOUND_STATUS: errorMessage += "Username or password combination wrong!\n";break;
			default: errorMessage += "Could not connect to the server!\n";break;
			}

			errorLabel.text = errorMessage;
		});
	}

	/* Checks validity of input and displays error message if any */
	public bool CheckInput () {
		errorLabel.text = "";
		errorLabel.text += Validator.isUsernameValid (username.text);
		errorLabel.text += Validator.isPasswordValid (password.text);

		return errorLabel.text.Equals ("");
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
