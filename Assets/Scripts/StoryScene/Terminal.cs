using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Terminal : MonoBehaviour {

	public GameObject terminalEntry;
	public GameObject scrollView;

	public GameObject terminal;
	public GameObject vim;
	public EventSystem eventSystem;
	public int numberOfEntries = 0;

	public void ExecuteCommand(Transform command) {
		string executable = command.GetComponent<InputField> ().text;
		executable += " ";
		string[] execuatableList = executable.Split(new char[] {' '});
		if (execuatableList [0].Equals ("vim") && execuatableList [1].Equals ("work.hs")) {
			terminal.SetActive (false);
			vim.SetActive (true);
			eventSystem.GetComponent<TerminalEventSystem> ().vimActive = true;
		} else {
			Debug.Log ("try again");
		}
	}

	public void CreateNewLine() {
		GameObject entry = GameObject.Instantiate (terminalEntry, new Vector3(10f,160f-(20*numberOfEntries),0f), Quaternion.identity);
		entry.transform.SetParent (transform.GetChild(0), false);
		if (numberOfEntries > 10) {
			scrollView.GetComponent<ScrollRect> ().verticalNormalizedPosition = 0;
			scrollView.GetComponent<ScrollRect> ().velocity = new Vector2 (0f, 100f);
		}
		numberOfEntries++;
	}
		
}
