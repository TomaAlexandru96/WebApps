using System;
using System.IO;
using UnityEngine;

public class CurrentUser : MonoBehaviour {
	
	private User userInfo = null;
	public const String userCache = "Assets/cache";
	private static CurrentUser instance = null;

	void Awake () {
		if (instance == null) {
			instance = this;
			LoadFromCache ();
		}
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

			SetUserInfo (JsonUtility.FromJson<User> (userInfoJSON));

			DBServer.GetInstance ().Login (userInfo.username, userInfo.password, false, (user) => { }, (error) => {
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

	private void ClearCahce () {
		WriteToCache ("");
	}

	public void Logout () {
		userInfo = null;
		ClearCahce ();
	}

	public User GetUserInfo () {
		return userInfo;
	}

	public void SetUserInfo (User userInfo) {
		this.userInfo = userInfo;
		SaveToCache ();
	}

	public bool IsLoggedIn () {
		return userInfo != null;
	}
}

