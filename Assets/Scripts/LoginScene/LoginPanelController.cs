using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanelController : MonoBehaviour {
	
	public GameObject username;
	public GameObject password;
	public GameObject mainPanel;

	public void requestLogin () {
		Debug.Log ("requested login");
	}

	public void goBack () {
		this.gameObject.SetActive (false);
		mainPanel.SetActive (true);
	}
}
