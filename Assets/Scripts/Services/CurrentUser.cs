using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUser : MonoBehaviour {
	
	private User userInfo = null;
	private Party party;
	public const String userCache = "Assets/cache";
	private bool withCaching = true;
	private static CurrentUser instance = null;

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
				RequestUpdate ((user) => {});
			}, (error) => {
				Logout (true);
				Debug.LogError (error);
			});
		}
	}

	public void OnApplicationQuit() {
		DBServer.GetInstance ().Logout (false, () => { }, (error) => {Debug.LogError (error);});
	}
		
	public static CurrentUser GetInstance () {
		return instance;
	}

	private void SaveToCache () {
		if (!withCaching) {
			return;
		}

		String userJSON = JsonUtility.ToJson (userInfo);
		WriteToCache (userJSON);
	}

	private void LoadFromCache () {
		if (!withCaching) {
			return;
		}

		try {
			StreamReader file = new StreamReader (userCache);
			String userInfoJSON = file.ReadToEnd ();

			if (userInfoJSON.Equals ("")) {
				return;
			}

			User loadedUser = JsonUtility.FromJson<User> (userInfoJSON);

			DBServer.GetInstance ().Login (loadedUser.username, loadedUser.password, false, (user) => {
			}, (error) => {
				Logout (true);
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

	public void Logout (bool overwriteCache) {
		LeaveParty ();
		userInfo = null;
		SetParty (null);
		CancelInvoke ();
		if (overwriteCache) {
			ClearCahce ();
		}
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

	public void RequestUpdate (Action<User> onFinish) {
		if (IsLoggedIn ()) {
			DBServer.GetInstance ().FindUser (userInfo.username, (user) => {
				SetUserInfo (user);
				onFinish (user);
			}, (error) => {
				Debug.LogError ("Something happened to the user: " + error);
				Logout (true);
			});
		}
	}

	public void SetParty (Party party) {
		this.party = party;
	}

	public Party GetParty () {
		return this.party;
	}

	public void LeaveParty () {
		if (this.party != null) {
			this.party.RequestLeaveParty ();
		}
	}

	public void SetWithCache (bool value) {
		this.withCaching = value;
	}
}

