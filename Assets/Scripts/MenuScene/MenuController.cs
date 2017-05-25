using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	
	private GameObject chat;
	private GameObject canvas;

	public void Start () {
		// restore chat
		chat = GameObject.FindGameObjectWithTag ("Chat");
		canvas = GameObject.FindGameObjectWithTag ("Canvas");
		chat.GetComponent <Transform> ().SetParent (canvas.GetComponent<Transform> ());
	}

	public void Logout () {
		DBServer.GetInstance ().Logout ();
		NetworkService.GetInstance ().DestroyConnection ();
		Destroy (GameObject.FindGameObjectWithTag ("Chat"));
		Destroy (GameObject.FindGameObjectWithTag ("NetworkEntity"));
		SceneManager.LoadScene ("Login");
	}

	public void PlayAdventure () {
		// move chat to adventure
		chat.GetComponent <Transform> ().SetParent (null);
		DontDestroyOnLoad (chat);
		SceneManager.LoadScene ("Adventure");
	}
}
