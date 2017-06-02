using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public Party party;
	private Action unsub;
	private Action unsub1;

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

		unsub1 = UpdateService.GetInstance ().Subscribe (UpdateType.PartyRequest, (sender, message) => {
			party.OnReceivedInvite (sender);
		});
	}

	public void OnDestroy () {
		unsub ();
		unsub1 ();
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
