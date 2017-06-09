using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentCollider : MonoBehaviour {

	public bool inside;
	public GameObject directionPanel;
	private float time;
	public string[] studentTexts;

	public void Start () {
		time = Time.time;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		inside = true;
	}

	void OnTriggerExit2D(Collider2D coll) {
		directionPanel.SetActive (false);
		inside = false;
	}

	public void Update() {
		if (inside && Input.GetKeyDown ("space") && Time.time - time > 0.5) {
			time = Time.time;
			directionPanel.SetActive (true);
			directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (GetRandomText());
		}
	}

	private string GetRandomText() {
		int maxText = studentTexts.Length;
		return studentTexts [Random.Range (0, maxText)];
	}
}	
