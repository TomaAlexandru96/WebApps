using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class terminalColliderScript : MonoBehaviour {

	public GameObject terminal;
	public EventSystem es;
	private bool canBeActivated = true;
	public GameObject instructionsPanel;

	void OnTriggerEnter2D(Collider2D coll) {
		if (canBeActivated) {
			terminal.SetActive (true);
			instructionsPanel.SetActive (true);
			es.GetComponent<TerminalEventSystem> ().SelectNextItem();
			canBeActivated = false;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().SetMovement (false);
			GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
	}
}
