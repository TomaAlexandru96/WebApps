using System;
using UnityEngine;

[System.Serializable]
public class User {

	public String username;
	public String password;
	public String email;
	public String[] friends;
	public String[] friend_requests;
	public bool active;
	public Character character;
	public PartyMembers party;

	public User (String username, String password, String email) {
		this.username = username;
		this.password = password;
		this.email = email;
	}

	public override String ToString () {
		return String.Format ("[username: {0}, \npassword: {1}, \nemail: {2}, \nfriends: {3}, \nfriend_requests: {4}, \nactive: {5}, \ncharacter: {6}, \nparty: {7}]\n",
			username, password, email, friends.ToString (), friend_requests.ToString (), active.ToString (), character.ToString (), party.ToString ());
	}

	public override bool Equals (object other) {
		if (!(other is User)) {
			return false;
		}

		User otherUser = (User) other;

		return username == otherUser.username &&
			password == otherUser.password &&
			email == otherUser.email &&
			friends.Equals (otherUser.friends) &&
			friend_requests.Equals (otherUser.friend_requests) &&
			active == otherUser.active;
	}

	public override int GetHashCode () {
		return base.GetHashCode ();
	}
}
