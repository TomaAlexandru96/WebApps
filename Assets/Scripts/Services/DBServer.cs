using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DBServer : MonoBehaviour {

	public const String DBServerAddr = "http://146.169.46.104:8081";
	public const long OK_STATUS = 200;
	public const long NOT_ACCEPTABLE_STATUS = 406;
	public const long NOT_FOUND_STATUS = 404;
	private static DBServer instance = null;

	void Awake () {
		if (instance == null) {
			DBServer.instance = this;	
		}
	}

	public static DBServer GetInstance () {
		return DBServer.instance;
	}
		
	/*  Issues login request to the DB server
		if no user is found for the following entries
		than it returns null
	*/
	public void Login (String username, String password, bool withEncription,
											Action<User> callback, Action<long> errorcall) {
		StartCoroutine (LoginHelper (username, password, withEncription, callback, errorcall));
		Debug.Log (CurrentUser.GetInstance ());
	}

	private IEnumerator<AsyncOperation> LoginHelper (String username, String password, bool withEncription,
										Action<User> callback, Action<long> errorcall) {
		if (withEncription) {
			password = DBServer.Encrypt (password);	
		}

		UnityWebRequest request = UnityWebRequest.Get (DBServerAddr + "/users?username=" +
												username + "&password=" + password);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			User userData = JsonUtility.FromJson<User> (request.downloadHandler.text);
			CurrentUser.GetInstance ().SetUserInfo (userData);
			StartCoroutine (SetUserActive (true));
			callback (userData);
		}
	}

	private IEnumerator<AsyncOperation> SetUserActive (bool active) {
		WWWForm form = new WWWForm ();
		form.AddField ("username", CurrentUser.GetInstance ().GetUserInfo ().username);
		form.AddField ("active", active.ToString ());

		UnityWebRequest request = UnityWebRequest.Post (DBServerAddr + "/set_active", form);

		yield return request.Send ();
	}

	/* Issues register request to DB server */
	public void Register (User user, Action<User> callback, Action<long> errorcall) {
		StartCoroutine (RegisterHelper (user, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> RegisterHelper (User user,
										Action<User> callback, Action<long> errorcall) {
		user.password = DBServer.Encrypt (user.password);

		WWWForm form = new WWWForm ();
		form.AddField ("username", user.username);
		form.AddField ("password", user.password);
		form.AddField ("email", user.email);

		UnityWebRequest request = UnityWebRequest.Post (DBServerAddr + "/register", form);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			User userData = JsonUtility.FromJson<User> (request.downloadHandler.text);
			CurrentUser.GetInstance ().SetUserInfo (userData);
			StartCoroutine (SetUserActive (true));
			callback (userData);
		}
	}

	/* Issues logout request to the DB server */
	public void Logout () {
		StartCoroutine (SetUserActive (false));
		CurrentUser.GetInstance ().Logout ();
	}

	public void FindUser (String username, Action<User> callback, Action<long> errorcall) {
		StartCoroutine (FindUserHelper (username, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> FindUserHelper (String username, Action<User> callback, Action<long> errorcall) {
		UnityWebRequest request = UnityWebRequest.Get (DBServerAddr + "/find_user?username=" + username);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			User userData = JsonUtility.FromJson<User> (request.downloadHandler.text);
			callback (userData);
		}
	}

	public void RequestFriend (String username, Action<User> callback, Action<long> errorcall) {
		StartCoroutine (RequestFriendHelper (username, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> RequestFriendHelper (String username, Action<User> callback, Action<long> errorcall) {
		WWWForm form = new WWWForm ();
		form.AddField ("user", CurrentUser.GetInstance ().GetUserInfo ().username);
		form.AddField ("requested_friend", username);

		UnityWebRequest request = UnityWebRequest.Post (DBServerAddr + "/request_friend", form);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			User otherUserData = JsonUtility.FromJson<User> (request.downloadHandler.text);
			callback (otherUserData);
		}
	}

	public void FindFriendsForUser (String username, Action<User[]> callback, Action<long> errorcall) {
		StartCoroutine (FindFriendsForUserHelper (username, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> FindFriendsForUserHelper (String username, Action<User[]> callback, Action<long> errorcall) {
		UnityWebRequest request = UnityWebRequest.Get (DBServerAddr + "/friends?username=" + username);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			User[] userData = JsonUtility.FromJson<User[]> (request.downloadHandler.text);
			callback (userData);
		}
	}

	public void FindFriendRequestsForUser (String username, Action<User[]> callback, Action<long> errorcall) {
		StartCoroutine (FindFriendRequestsForUserHelper (username, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> FindFriendRequestsForUserHelper (String username, Action<User[]> callback, Action<long> errorcall) {
		UnityWebRequest request = UnityWebRequest.Get (DBServerAddr + "/friend_requests?username=" + username);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			User[] userData = JsonUtility.FromJson<User[]> (request.downloadHandler.text);
			callback (userData);
		}
	}

	private static String Encrypt(String message) {
		return Convert.ToBase64String (Encoding.Unicode.GetBytes (message));
	}
}