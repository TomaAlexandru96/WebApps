using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TerminalEventSystem : MonoBehaviour {

	private EventSystem es;
	private Transform content;

	public GameObject terminalEntries;

	void Start () {
		es = GetComponent<EventSystem> ();
		SelectNextItem ();
		content = terminalEntries.transform.GetChild (0);
	}

	void Update () {
		if (Input.GetKeyUp (KeyCode.Return)) {
			terminalEntries.transform.GetComponent<Terminal> ().ExecuteCommand (GetLastEntry ());
			SelectNextItem ();
		}
	}

	private void SelectNextItem () {
		terminalEntries.transform.GetComponent<Terminal> ().CreateNewLine ();
		es.SetSelectedGameObject (GetLastEntry().gameObject);
	}

	private Transform GetLastEntry() {
		content = terminalEntries.transform.GetChild (0);
		int totalEntry = content.childCount;
		Debug.Log (totalEntry);
		return content.GetChild (totalEntry - 1).GetChild(1);
	}
}
