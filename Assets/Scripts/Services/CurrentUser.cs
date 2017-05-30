using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUser : MonoBehaviour {
	
	private User userInfo = null;
	public const String userCache = "Assets/cache";
	private static CurrentUser instance = null;
	private List<Notifiable> notifiables = new List<Notifiable> ();
	private PhotonPlayer pp;

	void Awake () {
		if (instance == null) {
			instance = this;
			LoadFromCache ();
			InvokeRepeating ("UpdateUserInfoCoroutine", 0, 5);
		}
	}

	public void Subscribe (Notifiable n) {
		notifiables.Add (n);
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
		pp = null;
		NetworkService.GetInstance ().DestroyConnection ();
		ClearCahce ();
	}

	public User GetUserInfo () {
		return userInfo;
	}

	public void SetUserInfo (User userInfo) {
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

	private void NotifyAll () {
		foreach (var n in notifiables) {
			n.Notify ();
		}
	}

	public void RequestUpdate () {
		if (IsLoggedIn ()) {
			DBServer.GetInstance ().FindUser (userInfo.username, (user) => {
				if (!user.Equals (userInfo)) {
					SetUserInfo (user);
					NotifyAll ();
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

