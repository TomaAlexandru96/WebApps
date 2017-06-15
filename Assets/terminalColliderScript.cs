using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class terminalColliderScript : MonoBehaviour {

	public GameObject terminal;
	public EventSystem es;
	private bool canBeActivated = true;

	void OnTriggerEnter2D(Collider2D coll) {
		if (canBeActivated) {
			terminal.SetActive (true);
			es.GetComponent<TerminalEventSystem> ().SelectNextItem();
			canBeActivated = false;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
	}
}
