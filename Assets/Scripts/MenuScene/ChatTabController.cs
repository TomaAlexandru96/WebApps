using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTabController : MonoBehaviour {

	public GameObject tabPrefab;
	private int totalChats = 0;

	public void AddChat() {
		GameObject newTab = Instantiate (tabPrefab);
		newTab.transform.SetParent (transform);
		totalChats++;
	} 
}
