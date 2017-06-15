using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class terminalColliderScript : MonoBehaviour {

	public GameObject terminal;
	public EventSystem es;

	void OnTriggerEnter2D(Collider2D coll) {
		terminal.SetActive (true);
		es.GetComponent<TerminalEventSystem> ().SelectNextItem();
	}

	void OnTriggerExit2D(Collider2D coll) {
	}
}
