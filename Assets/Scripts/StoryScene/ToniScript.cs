using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToniScript : MonoBehaviour {


	public GameObject directionPanel;
	public int maxSize;
	public string[] text;
	public int index = 0;
	public bool seated;
	protected bool firstContact;
	protected bool inside;
	protected float dateTime;

	public void Start () {
		dateTime = Time.time -1;
		firstContact = true;
		seated = false;
		directionPanel = GameObject.FindGameObjectsWithTag ("Canvas")[0].transform.GetChild(0).gameObject;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		inside = true;
	}

	void OnTriggerExit2D(Collider2D coll) {
		directionPanel.SetActive (false);
		inside = false;
	}

	protected virtual void Conversation() {
		directionPanel.SetActive (true);
		dateTime = Time.time;
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (text[index]);
		index++;
		firstContact = false;
	}

	public void Update () {
		if (!seated && (Time.time - dateTime) > 1.5 && index == 2 && inside){
			seated = true;
			index = 1;
			return;
		}
		if ((Input.GetKeyDown("space") || firstContact) && inside && index < maxSize &&  (Time.time - dateTime) > 0.5) {
			Conversation();
		}
		if (index >= maxSize && (Time.time - dateTime) > 1) {
			directionPanel.SetActive (false);
		}
	}
}
