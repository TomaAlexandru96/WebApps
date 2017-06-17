using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextInstructionScript : MonoBehaviour {

	public GameObject directionPanel;

	void OnTriggerEnter2D(Collider2D coll) {
	}

	void OnTriggerExit2D(Collider2D coll) {
		Debug.Log ("gets here");
		directionPanel.SetActive (true);
		StartCoroutine (DisplayMessage ());
	}

	private IEnumerator DisplayMessage () {
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText ("Well done ! Now go to the common room, you have a coursework due in a few minutes !");
		yield return new WaitForSeconds (3f);
		directionPanel.SetActive (false);
	}




}


