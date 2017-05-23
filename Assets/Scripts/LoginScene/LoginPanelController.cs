using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelController : MonoBehaviour {
	
	public InputField username;
	public InputField password;
	public GameObject mainPanel;

	public void RequestLogin () {
		Response response = DBServer.Login (username.text, password.text);
		Debug.Log (response);
	}

	public void GoBack () {
		this.gameObject.SetActive (false);
		mainPanel.SetActive (true);
	}
}
