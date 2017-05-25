using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdventureController : MonoBehaviour {
	
	private GameObject chat;
	private GameObject canvas;

	public void Start () {
		// restore chat
		chat = GameObject.FindGameObjectWithTag ("Chat");
		canvas = GameObject.FindGameObjectWithTag ("Canvas");
		chat.GetComponent <Transform> ().SetParent (canvas.GetComponent<Transform> ());
	}

	public void ExitGame () {
		// move chat to menu
		chat.GetComponent <Transform> ().SetParent (null);
		DontDestroyOnLoad (chat);
		SceneManager.LoadScene ("Menu");
	}
}
