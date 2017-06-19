using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextInstructionScript : MonoBehaviour {

	public GameObject directionPanel;
	public GameObject[] arrowsAcc = new GameObject[8];
	public GameObject[] arrowsDacc = new GameObject[8];

	void OnTriggerEnter2D(Collider2D coll) {
	}

	void OnTriggerExit2D(Collider2D coll) {
		directionPanel.SetActive (true);
		StartCoroutine (DisplayMessage ());
	}

	private IEnumerator DisplayMessage () {
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText ("Well done ! Now go to the common room, you have a coursework due in a few minutes !");
		yield return new WaitForSeconds (3f);
		foreach (GameObject arrow in arrowsAcc) {
			arrow.SetActive (true);
		}
		foreach (GameObject arrow in arrowsDacc) {
			arrow.SetActive (false);
		}
		directionPanel.SetActive (false);
	}




}


