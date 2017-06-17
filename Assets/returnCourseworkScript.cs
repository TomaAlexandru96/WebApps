using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class returnCourseworkScript : MonoBehaviour {

	public GameObject directionPanel;

	void OnTriggerEnter2D(Collider2D coll) {
		directionPanel.SetActive (true);
		StartCoroutine (DisplayMessage ());
	}

	void OnTriggerExit2D(Collider2D coll) {
	}

	private IEnumerator DisplayMessage () {
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText ("Huh, you made it in time ! Nice Work !");
		yield return new WaitForSeconds (2f);
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText ("It looks like your coursework has already been marked, go back to the common room to see what grade you got ! You don't need to hurry this time, it's all good");
		yield return new WaitForSeconds (2f);

		directionPanel.SetActive (false);


	}


}
