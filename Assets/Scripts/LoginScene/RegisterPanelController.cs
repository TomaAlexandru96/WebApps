using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanelController : MonoBehaviour {

	public InputField username;
	public InputField email;
	public InputField password;
	public InputField confirmPassword;
	public GameObject mainPanel;

	/* Used by register button to issue aregister query to the DB */
	public void RequestRegister () {
		User user = DBServer.Register (username.text, password.text, email.text);
	}

	/* Used by cancel button to go back to the main pane */
	public void GoBack () {
		this.gameObject.SetActive (false);
		mainPanel.SetActive (true);
	}
}
