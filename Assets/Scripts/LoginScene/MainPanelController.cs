using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanelController : MonoBehaviour {
	
	public GameObject loginPanel;
	public GameObject registerPanel;

	public void Start () {
	}


	/* Used by login button to change the pane to login pane */
	public void Login () {
		this.gameObject.SetActive (false);
		loginPanel.SetActive (true);
	}

	/* Used by registeer button to change the pane to register pane */
	public void Register () {
		this.gameObject.SetActive (false);
		registerPanel.SetActive (true);
	}
}
