using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class VimScript : MonoBehaviour {

	public GameObject terminalEntry;
	public GameObject scrollView;

	public GameObject terminal;
	public GameObject vim;
	public EventSystem eventSystem;
	public int numberOfEntries = 0;

	public bool isActive;

	public void Start() {
		isActive = false;
	}

	public void ExecuteCommand(string command) {
		if (command.Equals (":wq")) {
			terminal.SetActive (true);
			vim.SetActive (false);
			eventSystem.GetComponent<TerminalEventSystem> ().vimActive = false;
			eventSystem.GetComponent<TerminalEventSystem> ().SelectNextItem();
		} else {
			Debug.Log ("try again");
			eventSystem.GetComponent<TerminalEventSystem> ().resetVimCommand ();
		}
	}

	public void CreateNewLine() {
		if (isActive) {
			GameObject entry = GameObject.Instantiate (terminalEntry, new Vector3 (10f, 160f - (20 * numberOfEntries), 0f), Quaternion.identity);
			entry.transform.SetParent (transform.GetChild (0), false);
			if (numberOfEntries > 10) {
				scrollView.GetComponent<ScrollRect> ().verticalNormalizedPosition = 0;
				scrollView.GetComponent<ScrollRect> ().velocity = new Vector2 (0f, 100f);
			}
			numberOfEntries++;
		}
	}
}
