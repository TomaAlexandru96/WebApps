using System;
using System.Text.RegularExpressions;
using System.Net.Mail;
using UnityEngine;

public class Validator {

	public const string validPattern = @"^([a-zA-Z0-9]|\_|\.)+$";
	public const string emailValidPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

	public static String isUsernameValid (String username) {
		String error = "";

		if (username.Length == 0) {
			error = "Username should not be empty!\n";
		} else if (username.Length < 4) {
			error = "Username length should be greater than 4 characters!\n";
		} else if (!Regex.IsMatch (username, validPattern)) {
			error = "Username not valid! Valid pattern is "+validPattern+"\n";
		}

		return error;
	}

	public static String isPasswordValid (String password) {
		String error = "";

		if (password.Length == 0) {
			error = "Password should not be empty!\n";
		} else if (password.Length < 8) {
			error = "Password length should be greater than 8 characters!\n";
		} else if (!Regex.IsMatch (password, validPattern)) {
			error = "Password not valid! Valid pattern is "+validPattern+"\n";
		}

		return error;
	}

	public static String isEmailValid (String email) {
		String error = "";

		if (email.Length == 0) {
			error = "Email should not be empty!\n";
		} else if (!Regex.IsMatch (email, emailValidPattern)) {
			error = "Email is invalid!\n";
		}

		return error;
	}
}

