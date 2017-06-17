using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class printerScript : MonoBehaviour {

	public GameObject directionPanel;

	void OnTriggerEnter2D(Collider2D coll) {
		directionPanel.SetActive (true);
		StartCoroutine (DisplayMessage ());
	}

	void OnTriggerExit2D(Collider2D coll) {
	}

	private IEnumerator DisplayMessage () {
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText ("Printing....");
		yield return new WaitForSeconds (2f);
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText ("Still printing....");
		yield return new WaitForSeconds (2f);
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText ("Done !  RUN FOR THE SAO !");
		yield return new WaitForSeconds (2f);

		directionPanel.SetActive (false);


	}




}

