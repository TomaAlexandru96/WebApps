using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Terminal : MonoBehaviour {

	public GameObject terminalEntry;
	public int numberOfEntries = 0;

	public void ExecuteCommand(Transform command) {
		Debug.Log (command.GetComponent<InputField>().text);
	}

	public void CreateNewLine() {
		GameObject entry = GameObject.Instantiate (terminalEntry, new Vector3(10f,160f-(20*numberOfEntries),0f), Quaternion.identity);
		entry.transform.SetParent (transform.GetChild(0), false);
		if (numberOfEntries > 10) {
			transform.GetChild (0).GetComponent<RectTransform> ().localPosition = new Vector3 (258, numberOfEntries * (-10), 0);
		}
		numberOfEntries++;
	}
		
}
