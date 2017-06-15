using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TerminalEventSystem : MonoBehaviour {

	private EventSystem es;

	public bool vimActive;
	public bool writable;
	public GameObject terminalEntries;
	public GameObject vimEntries;
	public GameObject vimText;
	public GameObject vimCommand;

	public string inputText;

	void Start () {
		es = GetComponent<EventSystem> ();
		SelectNextItem ();
		vimActive = false;
		writable = false;
		vimCommand.SetActive (false);
	}

	void Update () {
		if (Input.GetKeyUp (KeyCode.Return)) {
			if (vimCommand.GetComponent<InputField> ().isFocused && vimActive) {
				Debug.Log ("uihsf");
				return;
			}

			if (vimActive){
				vimEntries.transform.GetComponent<VimScript> ().ExecuteCommand (GetLastEntry ());
			} else {
				terminalEntries.transform.GetComponent<Terminal> ().ExecuteCommand (GetLastEntry ());
			}
			SelectNextItem ();
		} 
		if (Input.GetKeyUp (KeyCode.I) && vimActive && !writable) {
			vimEntries.transform.GetComponent<VimScript> ().isActive = true;

			if ( vimEntries.transform.GetChild (0).childCount>0) {
				es.SetSelectedGameObject (GetLastEntry().gameObject);
			} else {
				SelectNextItem ();
			}

			vimCommand.SetActive (false);
			vimText.GetComponent<Text>().text = "INSERT";
			writable = true;
		}

		if (Input.GetKeyUp (KeyCode.I) && vimActive && !writable) {
			writable = true;

			vimCommand.SetActive (true);
			es.SetSelectedGameObject (vimCommand);
		}

		if (Input.GetKeyUp (KeyCode.Escape) && vimActive && writable) {
			GetLastEntry ().GetComponent<InputField> ().interactable = false;
			GetLastEntry ().GetComponent<InputField> ().text = inputText;


			vimEntries.transform.GetComponent<VimScript> ().isActive = false;
			writable = false;

			vimText.SetActive (false);
		}

		if (vimActive && vimEntries.transform.GetChild (0).childCount>0) {
			inputText = GetLastEntry ().GetComponent<InputField> ().text; 
		}
	}

	private void SelectNextItem () {
		if (vimActive) {
			vimEntries.transform.GetComponent<VimScript> ().CreateNewLine ();
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
			if (totalEntry > 0) {
				return vimEntries.transform.GetChild (0).GetChild (totalEntry - 1).GetChild (1);
			} 
			return vimEntries.transform;
		} else {
			totalEntry = terminalEntries.transform.GetChild (0).childCount;
			if (totalEntry > 1) {
				terminalEntries.transform.GetChild (0).GetChild (totalEntry - 2).GetChild (1).GetComponent<InputField> ().interactable = false;
			}
			return terminalEntries.transform.GetChild (0).GetChild (totalEntry - 1).GetChild (1);
		}
	}
}
