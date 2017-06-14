using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TerminalEventSystem : MonoBehaviour {

	private EventSystem es;

	public bool vimActive;
	public GameObject terminalEntries;
	public GameObject vimEntries;

	void Start () {
		es = GetComponent<EventSystem> ();
		SelectNextItem ();
		vimActive = false;
	}

	void Update () {
		if (Input.GetKeyUp (KeyCode.Return)) {
			if (vimActive){
				vimEntries.transform.GetComponent<Terminal> ().ExecuteCommand (GetLastEntry ());
			} else {
				terminalEntries.transform.GetComponent<Terminal> ().ExecuteCommand (GetLastEntry ());
			}
			SelectNextItem ();
		}
	}

	private void SelectNextItem () {
		if (vimActive) {
			vimEntries.transform.GetComponent<Terminal> ().CreateNewLine ();
		} else {
			terminalEntries.transform.GetComponent<Terminal> ().CreateNewLine ();
		}
		es.SetSelectedGameObject (GetLastEntry().gameObject);
	}

	private Transform GetLastEntry() {
		int totalEntry;
		if (vimActive) {
			totalEntry = vimEntries.transform.GetChild (0).childCount;
			if (totalEntry > 1) {
				vimEntries.transform.GetChild (0).GetChild (totalEntry - 2).GetChild (1).GetComponent<InputField> ().interactable = false;
			}
			return vimEntries.transform.GetChild (0).GetChild (totalEntry - 1).GetChild (1);
		} else {
			totalEntry = terminalEntries.transform.GetChild (0).childCount;
			if (totalEntry > 1) {
				terminalEntries.transform.GetChild (0).GetChild (totalEntry - 2).GetChild (1).GetComponent<InputField> ().interactable = false;
			}
			return terminalEntries.transform.GetChild (0).GetChild (totalEntry - 1).GetChild (1);
		}
	}
}
