using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TerminalEventSystem : MonoBehaviour {

	private EventSystem es;

	public bool vimActive;
	public bool writable;
	public bool writable2;
	public GameObject terminalEntries;
	public GameObject vimEntries;
	public GameObject vimText;
	public GameObject vimCommand;

	public string inputText;

	void Start () {
		es = GetComponent<EventSystem> ();
		vimActive = false;
		writable = false;
		writable2 = false;
		vimCommand.SetActive (false);
	}

	void Update () {
		if (Input.GetKeyUp (KeyCode.Return)) {
			if (writable2 && vimActive) {
				es.SetSelectedGameObject (vimCommand);
				vimEntries.GetComponent<VimScript> ().ExecuteCommand (vimCommand.GetComponent<InputField>().text);
				return;
			}

			if (vimActive){
				vimEntries.transform.GetComponent<VimScript> ().ExecuteCommand ("");
			} else {
				terminalEntries.transform.GetComponent<Terminal> ().ExecuteCommand (GetLastEntry ());
			}
			SelectNextItem ();
		} 
		if (Input.GetKeyUp (KeyCode.I) && vimActive && !writable) {
			vimEntries.transform.GetComponent<VimScript> ().isActive = true;

			if ( vimEntries.transform.GetChild (0).childCount>0) {
				GetLastEntry ().GetComponent<InputField> ().interactable = true;
				es.SetSelectedGameObject (GetLastEntry().gameObject);
			} else {
				SelectNextItem ();
			}

			vimCommand.SetActive (false);
			vimText.GetComponent<Text>().text = "INSERT";
			writable = true;
		}

		if (Input.GetKeyDown (";") && vimActive && !writable) {
			writable2 = true;
			writable = true;
			vimCommand.SetActive (true);
			es.SetSelectedGameObject (vimCommand);
			vimCommand.GetComponent<InputField> ().text = ":";
		}

		if (Input.GetKeyUp (KeyCode.Escape) && vimActive && (writable || writable2)) {
			GetLastEntry ().GetComponent<InputField> ().interactable = false;
			GetLastEntry ().GetComponent<InputField> ().text = inputText;


			vimEntries.transform.GetComponent<VimScript> ().isActive = false;
			writable = false;
			writable2 = false;

			vimText.SetActive (false);
			vimCommand.SetActive (false);
		}

		if (vimActive && vimEntries.transform.GetChild (0).childCount>0) {
			if (!GetLastEntry ().GetComponent<InputField> ().text.Equals ("")) {
				inputText = GetLastEntry ().GetComponent<InputField> ().text; 
			} 
		}
	}

	public void SelectNextItem () {
		Debug.Log ("called");

		if (vimActive) {
			vimEntries.transform.GetComponent<VimScript> ().CreateNewLine ();
		} else {
			terminalEntries.transform.GetComponent<Terminal> ().CreateNewLine ();
		}
		es.SetSelectedGameObject (GetLastEntry().gameObject);
	}

	public  Transform GetLastEntry() {
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

	public void resetVimCommand() {
		vimCommand.GetComponent<InputField> ().text = "";
		es.SetSelectedGameObject (vimCommand);
	}
}
