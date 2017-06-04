using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Experimental;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public class DBServer : MonoBehaviour {

	public const String DBServerAddr = "https://cloud-vm-46-104.doc.ic.ac.uk:8000";
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
	public void Logout (bool overwriteCaching, Action callback, Action<long> errorcall) {
		StartCoroutine (LogoutHelper (overwriteCaching, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> LogoutHelper (bool overwriteCaching, Action callback, Action<long> errorcall) {
		WWWForm form = new WWWForm ();
		form.AddField ("username", CurrentUser.GetInstance ().GetUserInfo ().username);

		UnityWebRequest request = UnityWebRequest.Post (DBServerAddr + "/logout", form);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			string[] targets = CurrentUser.GetInstance ().GetUserInfo ().friends;
				
			if (CurrentUser.GetInstance ().IsInParty ()) {
				targets = new string[CurrentUser.GetInstance ().GetUserInfo ().friends.Length + 
					CurrentUser.GetInstance ().GetUserInfo ().party.partyMembers.Length];
				CurrentUser.GetInstance ().GetUserInfo ().friends.CopyTo (targets, 0);
				CurrentUser.GetInstance ().GetUserInfo ().party.partyMembers.CopyTo (targets, 
					CurrentUser.GetInstance ().GetUserInfo ().friends.Length);
			}

			UpdateService.GetInstance ().SendUpdate (targets,
							UpdateService.CreateMessage (UpdateType.LogoutUser));
			NetworkService.GetInstance ().LeaveRoom ();
			CurrentUser.GetInstance ().Logout (overwriteCaching);
			callback ();
		}
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
			CurrentUser.GetInstance ().RequestUpdate ((userInfo) => {
				callback ();
			});
		}
	}

	private static String Encrypt(String message) {
		return Convert.ToBase64String (Encoding.Unicode.GetBytes (message));
	}

	public void CreateParty (String owner, int mode, Action callback, Action<long> errorcall) {
		StartCoroutine (CreatePartyHelper (owner, mode, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> CreatePartyHelper (String owner, int mode, Action callback, Action<long> errorcall) {
		WWWForm form = new WWWForm ();
		form.AddField ("owner", owner);
		form.AddField ("mode", mode);

		UnityWebRequest request = UnityWebRequest.Post (DBServerAddr + "/create_party", form);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			CurrentUser.GetInstance ().RequestUpdate ((user) => {
				NetworkService.GetInstance ().JoinLobby (mode);
				NetworkService.GetInstance ().CreateRoom (CurrentUser.GetInstance ().GetUserInfo ().username);
				callback ();
			});
		}
	}
		
	public void JoinParty (String owner, String username, int mode, Action callback, Action<long> errorcall) {
		StartCoroutine (JoinPartyHelper (owner, username, mode, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> JoinPartyHelper (String owner, String username, int mode, Action callback, Action<long> errorcall) {
		WWWForm form = new WWWForm ();
		form.AddField ("owner", owner);
		form.AddField ("username", username);

		UnityWebRequest request = UnityWebRequest.Post (DBServerAddr + "/join_party", form);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			CurrentUser.GetInstance ().RequestUpdate ((user) => {
				UpdateService.GetInstance ().SendUpdate (CurrentUser.GetInstance ().GetUserInfo ().party.partyMembers,
						UpdateService.CreateMessage (UpdateType.PartyRequestAccept));
				NetworkService.GetInstance ().JoinLobby (mode);
				NetworkService.GetInstance ().JoinRoom (CurrentUser.GetInstance ().GetUserInfo ().username);
				callback ();
			});
		}
	}

	public void LeaveParty (String username, Action callback, Action<long> errorcall) {
		StartCoroutine (LeavePartyHelper (username, callback, errorcall));
	}

	private IEnumerator<AsyncOperation> LeavePartyHelper (String username, Action callback, Action<long> errorcall) {
		WWWForm form = new WWWForm ();
		form.AddField ("username", username);

		UnityWebRequest request = UnityWebRequest.Post (DBServerAddr + "/leave_party", form);

		yield return request.Send ();

		if (request.responseCode != OK_STATUS) {
			errorcall (request.responseCode);
		} else {
			UpdateService.GetInstance ().SendUpdate (CurrentUser.GetInstance ().GetUserInfo ().party.partyMembers, 
								UpdateService.CreateMessage (UpdateType.PartyLeft));
			CurrentUser.GetInstance ().RequestUpdate ((user) => {
				NetworkService.GetInstance ().LeaveRoom ();
				callback ();
			});
		}
	}
}
