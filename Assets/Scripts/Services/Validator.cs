using System;
using System.Text.RegularExpressions;
using System.Net.Mail;

public class Validator {

	public static String isUsernameValid (String username) {
		String error = "";

		if (username.Length == 0) {
			error = "Username should not be empty!\n";
		} else if (username.Length < 4) {
			error = "Username length should be greater than 4 characters!\n";
		} else if (username.Contains (" ±!@£$%^&*()_+§[]\\';,./{}|\":?><")) {
			error = "Username not valid!\n";
		} else {
			return error;
		}

		return error;
	}

	public static String isPasswordValid (String password) {
		String error = "";

		if (password.Length == 0) {
			error = "Password should not be empty!\n";
		} else if (password.Length < 8) {
			error = "Password length should be greater than 8 characters!\n";
		} else if (password.Contains (" ±!@£$%^&*()_+§[]\\';,./{}|\":?><")) {
			error = "Password not valid!\n";
		} else {
			return error;
		}

		return error;
	}

	public static String isEmailValid (String email) {
		String error = "";

		if (email.Length == 0) {
			error = "Email should not be empty!\n";
			return error;
		}

		try {
			new MailAddress (email);
		} catch (FormatException _) {
			error = "Email format invalid!\n";
		}

		return error;
	}
}

