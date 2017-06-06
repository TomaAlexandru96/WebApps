using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTabController : MonoBehaviour {

	public GameObject tabPrefab;
	public GameObject content;

	public void AddChat (String name, String viewName, bool isCloseable) {
		GameObject newTab = (GameObject) Instantiate (tabPrefab);
		newTab.transform.SetParent (content.transform);
		newTab.GetComponent <ChatTab> ().UpdateName (name, viewName);
		newTab.GetComponent <ChatTab> ().isCloseable = isCloseable;
		ActivateLastTab ();
	}

	private void ActivateLastTab () {
		content.transform.GetChild (content.transform.childCount - 1).GetComponent<ChatTab> ().SelectChat ();
	}

	public void DestroyChat (string chatName) {
		foreach (var tab in content.transform.GetComponentsInChildren<ChatTab> ()) {
			if (tab.GetName ().Equals (chatName)) {
				DestroyImmediate (tab.gameObject);
				break;
			}
		}
		ActivateLastTab ();
	}

	public bool ChatAlreadyExist (String name) {
		foreach (Transform child in content.transform) {
			if (child.GetComponent <ChatTab> ().GetName ().Equals (name)) {
				return true;
			}
		}
		return false;
	}
}
