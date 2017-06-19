using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class returnCourseworkScript : MonoBehaviour {

	public GameObject directionPanel;
	public GameObject[] arrowsAcc = new GameObject[8];
	public GameObject[] arrowsDacc = new GameObject[8];

	void OnTriggerEnter2D(Collider2D coll) {
		directionPanel.SetActive (true);
		StartCoroutine (DisplayMessage ());
	}

	void OnTriggerExit2D(Collider2D coll) {
	}

	private IEnumerator DisplayMessage () {
		foreach (GameObject arrow in arrowsAcc) {
			arrow.SetActive (true);
		}
		foreach (GameObject arrow in arrowsDacc) {
			arrow.SetActive (false);
		}
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText ("Huh, you made it in time ! Nice Work !");
		yield return new WaitForSeconds (2f);
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText ("It looks like your coursework has already been marked, go back to the common room to see what grade you got!");
		yield return new WaitForSeconds (4f);
		directionPanel.SetActive (false);


	}


}
