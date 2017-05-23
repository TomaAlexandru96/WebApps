using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
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

		Response<User> response = DBServer.Login (username.text, password.text);

		if (response.error != null) {
			if (response.error.Status == WebExceptionStatus.ConnectFailure) {
				errorLabel.text = "Could not connect to the server!\n";
			} else if (response.error.Status == WebExceptionStatus.ReceiveFailure) {
				errorLabel.text = "Username or password combination wrong!\n";
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
