using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonRoomScript : MonoBehaviour {

	public GameObject courseworkPanel;
	public bool doneCoursework;

	public void Start () {
		courseworkPanel.SetActive (false);
		doneCoursework = false;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (!doneCoursework) {
			courseworkPanel.SetActive (true);
			doneCoursework = true;
		} else {
			Debug.Log ("gets here");
		}
	}

}
