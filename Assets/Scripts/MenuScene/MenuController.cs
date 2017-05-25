using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public void Logout () {
		DBServer.GetInstance ().Logout ();
		Destroy (GameObject.FindGameObjectWithTag ("ChatCanvas"));
		Destroy (GameObject.FindGameObjectWithTag ("NetworkEntity"));
		SceneManager.LoadScene ("Login");
	}

	public void Play () {
		SceneManager.LoadScene ("Adventure");
	}
}
