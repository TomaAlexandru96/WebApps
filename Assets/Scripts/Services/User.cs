using System;

public class User {
	
	public String username;
	public String password;
	public String email;
	public String[] friends;
	public String[] friend_requests;
	public bool active;

	public User (String username, String password, String email) {
		this.username = username;
		this.password = password;
		this.email = email;
	}

	public override String ToString () {
		return String.Format ("[username: {0}, password: {1}, email: {2}, friends: {3}, friend_requests: {4}, active: {5}]",
			username, password, email, friends.ToString (), friend_requests.ToString (), active.ToString ());
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

	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}
}

