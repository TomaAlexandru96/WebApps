using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DBServer : MonoBehaviour {

	public const String DBServerAddr = "http://146.169.46.104:8000";
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
	}

	private IEnumerator<AsyncOperation> LoginHelper (String username, String password, bool withEncription,
										Action<User> callback, Action<long> errorcall) {
		if (withEncription) {
			password = DBServer.Encrypt (password);
		}

		WWWForm form = new WWWForm ();
		form.AddField ("username", username);
		form.AddField ("password", password);

		UnityWebRequest request = UnityWebRequest.Post (DBServerAddr + "/login", form);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			User userData = JsonUtility.FromJson<User> (request.downloadHandler.text);
			CurrentUser.GetInstance ().Login (userData);
			callback (userData);
			ChatService.GetInstance ().StartService ();
			UpdateService.GetInstance ().StartService ();
			NetworkService.GetInstance ().StartService ();
		}
	}

	/* Issues register request to DB server */
	public void Register (User user, Action callback, Action<long> errorcall) {
		StartCoroutine (RegisterHelper (user, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> RegisterHelper (User user,
										Action callback, Action<long> errorcall) {
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
			callback ();
		}
	}

	/* Issues logout request to the DB server */
	public void Logout (Action callback, Action<long> errorcall) {
		SetActiveStatus (false, () => {
			// logout
			UpdateService.GetInstance ().SendUpdate (CurrentUser.GetInstance ().GetUserInfo ().friends,
					UpdateService.CreateMessage (UpdateType.LogoutUser));
			callback ();
			CurrentUser.GetInstance ().LeaveParty ();
			CurrentUser.GetInstance ().ClearCahce ();
			CurrentUser.GetInstance ().Logout ();
			NetworkService.GetInstance ().StopService ();
			UpdateService.GetInstance ().StopService ();
			ChatService.GetInstance ().StopService ();
		}, errorcall);
	}

	public void SetActiveStatus (bool status, Action callback, Action<long> errorcall) {
		StartCoroutine (SetActiveStatusHelper(status, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> SetActiveStatusHelper (bool status, Action callback, Action<long> errorcall) {
		WWWForm form = new WWWForm ();
		form.AddField ("username", CurrentUser.GetInstance ().GetUserInfo ().username);
		form.AddField ("active", status.ToString());

		UnityWebRequest request = UnityWebRequest.Post (DBServerAddr + "/update_active_status", form);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			CurrentUser.GetInstance ().RequestUpdate ((user) => {
				callback ();
			});
		}
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

	public void RequestFriend (String username, Action callback, Action<long> errorcall) {
		StartCoroutine (RequestFriendHelper (username, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> RequestFriendHelper (String username, Action callback, Action<long> errorcall) {
		WWWForm form = new WWWForm ();
		form.AddField ("user", CurrentUser.GetInstance ().GetUserInfo ().username);
		form.AddField ("requested_friend", username);

		UnityWebRequest request = UnityWebRequest.Post (DBServerAddr + "/request_friend", form);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			callback ();
		}
	}

	public void AcceptFriendRequest (String username, Action callback, Action<long> errorcall) {
		StartCoroutine (AcceptFriendRequestHelper (username, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> AcceptFriendRequestHelper (String username, Action callback, Action<long> errorcall) {
		WWWForm form = new WWWForm ();
		form.AddField ("user", CurrentUser.GetInstance ().GetUserInfo ().username);
		form.AddField ("requested_friend", username);

		UnityWebRequest request = UnityWebRequest.Post (DBServerAddr + "/accept_friend_request", form);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			callback ();
		}
	}

	public void RejectFriendRequest (String username, Action callback, Action<long> errorcall) {
		StartCoroutine (RejectFriendRequestHelper (username, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> RejectFriendRequestHelper (String username, Action callback, Action<long> errorcall) {
		WWWForm form = new WWWForm ();
		form.AddField ("user", CurrentUser.GetInstance ().GetUserInfo ().username);
		form.AddField ("requested_friend", username);

		UnityWebRequest request = UnityWebRequest.Post (DBServerAddr + "/reject_friend_request", form);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			callback ();
		}
	}

	public void ChooseCharacter (User user, String characterName, int id, Action callback, Action<long> errorcall) {
		StartCoroutine (ChooseCharacterHelper (user, characterName, id, callback, errorcall));
	}

	public IEnumerator<AsyncOperation> ChooseCharacterHelper (User user, String characterName, int id,
																Action callback, Action<long> errorcall) {

		WWWForm form = new WWWForm ();
		form.AddField ("username", user.username);
		form.AddField ("characterName", characterName);
		form.AddField ("characterID", id);

		UnityWebRequest request = UnityWebRequest.Post (DBServerAddr + "/chooseCharacter", form);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			callback ();
		}
	}

	private static String Encrypt(String message) {
		return Convert.ToBase64String (Encoding.Unicode.GetBytes (message));
	}
}
