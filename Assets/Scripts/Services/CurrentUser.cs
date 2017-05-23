using System;
using System.IO;
using UnityEngine;

public class CurrentUser {
	
	private static CurrentUser instance = new CurrentUser();
	private User userInfo = null;
	public const String userCache = "Assets/cache";

	private CurrentUser () {
		LoadFromCache ();
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
			userInfo = JsonUtility.FromJson<User> (userInfoJSON);

			file.Close ();
		} catch (Exception _) {
			return;
		}
	}

	private void WriteToCache (String message) {
		try {
			StreamWriter file = new StreamWriter (userCache);
			file.WriteLine (message);
			file.Close ();
		} catch (Exception _) {
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

	public static CurrentUser GetInstance () {
		return instance;
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

