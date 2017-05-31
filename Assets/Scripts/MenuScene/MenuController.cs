using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	private Action unsub1;
	private Action unsub2;

	public void Awake () {
		unsub1 = UpdateService.GetInstance ().Subscribe (UpdateType.LoginUser, (sender, message) => {
			if (!CurrentUser.GetInstance ().IsLoggedIn ()) {
				Logout ();
			}
		});

		unsub2 = UpdateService.GetInstance ().Subscribe (UpdateType.LogoutUser, (sender, message) => {
			if (!CurrentUser.GetInstance ().IsLoggedIn ()) {
				Logout ();
			}
		});
	}

	public void Start () {
	}

	public void OnDestroy () {
		unsub1 ();
		unsub2 ();
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
