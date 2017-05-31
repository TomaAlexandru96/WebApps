using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUser : MonoBehaviour {
	
	private User userInfo = null;
	public const String userCache = "Assets/cache";
	private static CurrentUser instance = null;
	private PhotonPlayer pp;

	public void Awake () {
		if (instance == null) {
			instance = this;
			InvokeRepeating ("UpdateOnlineStatus", 0, 60);
			LoadFromCache ();
		}
	}

	public void UpdateOnlineStatus () {
		if (IsLoggedIn ()) {
			DBServer.GetInstance ().SetActiveStatus (true, () => {			
			}, (error) => {
				Logout ();
				Debug.LogError (error);
			});
		}
	}

	public void OnApplicationQuit() {
		DBServer.GetInstance ().Logout (() => { }, (error) => {Debug.LogError (error);});
	}
		
	public static CurrentUser GetInstance () {
		return instance;
	}

	private void SaveToCache () {
		String userJSON = JsonUtility.ToJson (userInfo);
		WriteToCache (userJSON);
	}

	private void LoadFromCache () {
		try {
			StreamReader file = new StreamReader (userCache);
			String userInfoJSON = file.ReadToEnd ();

			if (userInfoJSON.Equals ("")) {
				return;
			}

			User loadedUser = JsonUtility.FromJson<User> (userInfoJSON);

			DBServer.GetInstance ().Login (loadedUser.username, loadedUser.password, false, (user) => {
			}, (error) => {
				Logout ();
			});

			file.Close ();
		} catch (Exception) {
			return;
		}
	}

	private void WriteToCache (String message) {
		try {
			StreamWriter file = new StreamWriter (userCache);
			file.WriteLine (message);
			file.Close ();
		} catch (Exception) {
			return;
		}
	}

	public void ClearCahce () {
		WriteToCache ("");
	}

	public void Login (User userInfo) {
		SetUserInfo (userInfo);
	}

	public void Logout () {
		userInfo = null;
		pp = null;
		CancelInvoke ();
	}

	public User GetUserInfo () {
		return userInfo;
	}

	private void SetUserInfo (User userInfo) {
		this.userInfo = userInfo;
		this.userInfo.active = true;
		SaveToCache ();
	}

	public bool IsLoggedIn () {
		return userInfo != null;
	}

	public override String ToString () {
		if (userInfo != null) {
			return userInfo.ToString ();
		} else {
			return "Not Logged";
		}
	}

	public void RequestUpdate () {
		if (IsLoggedIn ()) {
			DBServer.GetInstance ().FindUser (userInfo.username, (user) => {
				if (!user.Equals (userInfo)) {
					SetUserInfo (user);
				}
			}, (error) => {
				Debug.LogError ("Something happened to the user: " + error);
				Logout ();
			});
		}
	}

	public void JoinParty () {
		// pp = new PhotonPlayer ();
	}
}

