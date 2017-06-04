using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour {

	public GameObject optionPanel;

	public void start() {
		optionPanel.SetActive (false);
	}

	public void OptionButtonClicked() {
		if (optionPanel.activeSelf) {
			optionPanel.SetActive (false);
		} else {
			optionPanel.SetActive (true);
		}
	}
}
