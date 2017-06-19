using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class terminalColliderScript : MonoBehaviour {

	public GameObject terminal;
	public GameObject floor;
	public EventSystem es;
	private bool canBeActivated = true;
	public GameObject instructionsPanel;
	public Sprite badSprite;

	void OnTriggerEnter2D(Collider2D coll) {
		if (canBeActivated) {
			terminal.SetActive (true);
			floor.GetComponent<SpriteRenderer> ().sprite = badSprite;
			instructionsPanel.SetActive (true);
			es.GetComponent<TerminalEventSystem> ().SelectNextItem();
			canBeActivated = false;
			GameObject.FindGameObjectWithTag ("Canvas").GetComponent<CanvasScript> ().RemovePartyPanel ();
			GameObject.FindGameObjectWithTag ("Labs").GetComponent<Labs> ().RemoveStudents ();
			GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().SetMovement (false);
			GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
	}
}
