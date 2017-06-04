using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public GameObject[] UIPanels;
	public Party party;
	private bool isAdventure = true;
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

	public void CreateParty () {
		DBServer.GetInstance ().CreateParty (CurrentUser.GetInstance ().GetUserInfo ().username, () => {
			party.Join ();
		}, (error) => {
			Debug.LogError (error);
		});
	}

	public void SwitchToJoinView () {
		ClearView ();
		UIPanels [6].SetActive (true);
	}

	public void SwitchToPartyView () {
		ClearView ();
		UIPanels [2].SetActive (true);
	}

	public void SwtichToMenuView () {
		ClearView ();
		UIPanels [0].SetActive (true);
	}

	public void ClearView () {
		foreach (var panel in UIPanels) {
			panel.SetActive (false);
		}
	}

	public void SetIsAdventure (bool value) {
		this.isAdventure = value;
		if (this.isAdventure) {
			NetworkService.GetInstance ().JoinAdventureLobby ();
		} else {
			NetworkService.GetInstance ().JoinEndlessLobby ();
		}
	}

	public bool GetIsAdventure () {
		return isAdventure;
	}
}
