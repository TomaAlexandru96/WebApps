using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour, Notifiable {

	public void Notify () {
		if (!CurrentUser.GetInstance ().IsLoggedIn ()) {
			Logout ();
		}
	}

	public void Logout () {
		DBServer.GetInstance ().Logout ();
		NetworkService.GetInstance ().DestroyConnection ();
		Destroy (GameObject.FindGameObjectWithTag ("NetworkEntity"));
		Destroy (GameObject.FindGameObjectWithTag ("ChatEntity"));
		Destroy (GameObject.FindGameObjectWithTag ("UpdateEntity"));
		SceneManager.LoadScene ("Login");
	}

	public void PlayAdventure () {
		SceneManager.LoadScene ("Adventure");
	}
}
