using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	private Action unsub;

	public void Awake () {
		// start services 
		UpdateService.GetInstance ().StartService ();
		ChatService.GetInstance ().StartService (() => {
			NetworkService.GetInstance ().StartService ();
			UpdateService.GetInstance ().SendUpdate (CurrentUser.GetInstance ().GetUserInfo ().friends, 
				UpdateService.CreateMessage (UpdateType.LoginUser));
		});

		unsub = UpdateService.GetInstance ().Subscribe (UpdateType.UserUpdate, (sender, message) => {
			if (!CurrentUser.GetInstance ().IsLoggedIn ()) {
				Logout ();
			}
		});
	}

	public void OnDestroy () {
		unsub ();
	}

	public void Logout () {
		DBServer.GetInstance ().Logout (true, () => {
			NetworkService.GetInstance ().StopService ();
			UpdateService.GetInstance ().StopService ();
			ChatService.GetInstance ().StopService ();
			SceneManager.LoadScene ("Login");	
		}, (error) => {
			Debug.LogError (error);
		});
	}

	public void PlayAdventure () {
		SceneManager.LoadScene ("Adventure");
	}
}
