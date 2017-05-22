using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanelController : MonoBehaviour {
	
	public GameObject loginPanel;
	public GameObject registerPanel;

	public void Start () {
		Debug.Log ("You need to login");
	}

	public void login () {
		this.gameObject.SetActive (false);
		loginPanel.SetActive (true);
	}

	public void register () {
		this.gameObject.SetActive (false);
		registerPanel.SetActive (true);
	}
}
