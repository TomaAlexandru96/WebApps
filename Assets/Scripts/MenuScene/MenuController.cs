using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public void Logout () {
		DBServer.GetInstance ().Logout ();
		NetworkService.GetInstance ().DestroyConnection ();
		Destroy (GameObject.FindGameObjectWithTag ("NetworkEntity"));
		SceneManager.LoadScene ("Login");
	}

	public void PlayAdventure () {
		SceneManager.LoadScene ("Adventure");
	}
}
