using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour {

	float time;
	private bool event1;
	private bool event2;

	void Start () {
		time = Time.time;
		event1 = true;
		event2 = true;
	}

	void Update () {

		if (event1 && Time.time > time) {
			event1 = false;
			GameObject.FindGameObjectWithTag ("Chat").SetActive (false);
			GameObject.FindGameObjectWithTag ("PlayerAbilities").SetActive (false);
			GameObject.FindGameObjectWithTag ("DirectionPanel").SetActive (false);
		}

		if (event2 && Time.time - time > 3) {
			event2 = false;
			GameObject.FindGameObjectWithTag ("PartMention").SetActive (false);
			GameObject.FindGameObjectWithTag ("Canvas").transform.GetChild (0).gameObject.SetActive (true);
		}
	}
}
