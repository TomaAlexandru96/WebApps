using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterPanelController : MonoBehaviour {

	public GameObject username;
	public GameObject email;
	public GameObject password;
	public GameObject confirmPassword;
	public GameObject mainPanel;

	public void requestRegister () {
		Debug.Log ("requested register");
	}

	public void goBack () {
		this.gameObject.SetActive (false);
		mainPanel.SetActive (true);
	}
}
