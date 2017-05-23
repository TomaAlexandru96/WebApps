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

	public void RequestRegister () {
		Debug.Log ("requested register");
	}

	public void GoBack () {
		this.gameObject.SetActive (false);
		mainPanel.SetActive (true);
	}
}
