using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	private Action unsub;

	public void Awake () {
		unsub = UpdateService.GetInstance ().Subscribe (UpdateType.UserUpdate, (sender, message) => {
			if (!CurrentUser.GetInstance ().IsLoggedIn ()) {
				Logout ();
			}
		});
	}

	public void Start () {
	}

	public void OnDestroy () {
		unsub ();
	}

	public void Logout () {
		DBServer.GetInstance ().Logout (() => {
			NetworkService.GetInstance ().DestroyConnection ();
			Destroy (GameObject.FindGameObjectWithTag ("NetworkEntity"));
			Destroy (GameObject.FindGameObjectWithTag ("ChatEntity"));
			Destroy (GameObject.FindGameObjectWithTag ("UpdateEntity"));
			CurrentUser.GetInstance ().ClearCahce ();
			SceneManager.LoadScene ("Login");	
		}, (error) => {
			Debug.LogError (error);
		});
	}

	public void PlayAdventure () {
		SceneManager.LoadScene ("Adventure");
	}
}
