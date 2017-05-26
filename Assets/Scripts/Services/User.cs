using System;

public class User {
	
	public int id;
	public String username;
	public String password;
	public String email;
	public int[] friends;
	public int[] friend_requests;
	public bool active;

	public User (String username, String password, String email) {
		this.username = username;
		this.password = password;
		this.email = email;
	}

	public override String ToString () {
		return String.Format ("[id: {0}, username: {1}, password: {2}, email: {3}, friends: {4}, friend_requests: {5}, active: {6}]",
			id, username, password, email, friends.ToString (), friend_requests.ToString (), active.ToString ());
	}
}

