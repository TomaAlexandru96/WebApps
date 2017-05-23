using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventController : MonoBehaviour {

	public GameObject mainPanel;
	public GameObject loginPanel;
	public GameObject registerPanel;
	private EventSystem es;

	public GameObject[] mainPanelButtons;
	public GameObject[] loginPanelButtons;
	public GameObject[] registerPanelButtons;
	private Dictionary<GameObject, GameObject[]> panelButtons;
	private int activeIndex = 0;

	public void Start () {
		es = GetComponent<EventSystem> ();
		panelButtons = new Dictionary<GameObject, GameObject[]> () {
			{mainPanel, mainPanelButtons},
			{loginPanel, loginPanelButtons},
			{registerPanel, registerPanelButtons}
		};
	}

	public GameObject GetActivePanel () {
		if (mainPanel.GetActive ()) {
			return mainPanel;
		} else if (loginPanel.GetActive ()) {
			return loginPanel;
		} else {
			return registerPanel;
		}
	}

	public void SelectFirstItem () {
		GameObject active = GetActivePanel ();
		GameObject[] activeButtons = panelButtons[active];

		activeIndex = 0;
		es.SetSelectedGameObject (activeButtons[activeIndex]);
	}

	public void Update () {
		if (Input.GetKeyUp ("tab")) {
			SelectNextItem ();
		}
	}

	private void SelectNextItem () {
		GameObject active = GetActivePanel ();
		GameObject[] activeButtons = panelButtons[active];

		activeIndex = (activeIndex + activeButtons.Length + 1) % activeButtons.Length;
		es.SetSelectedGameObject (activeButtons[activeIndex]);
	}
}
