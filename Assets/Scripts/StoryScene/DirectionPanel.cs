using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionPanel : MonoBehaviour {

	public GameObject textPanel;

	public void DisplayText(string text) {
		textPanel.transform.GetComponent<Text> ().text = text;
	}
}
