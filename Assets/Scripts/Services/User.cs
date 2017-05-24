using System;

public class User {
	
	public int id;
	public String username;
	public String password;
	public String email;

	public User (String username, String password, String email) {
		this.username = username;
		this.password = password;
		this.email = email;
	}
}

