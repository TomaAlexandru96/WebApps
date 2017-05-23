using System;
using System.Net;
using System.Text;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;

public class DBServer {

	public const String DBServerAddr = "http://146.169.46.104:8081";

	private DBServer () {
	}
		
	/*  Issues login request to the DB server
		if no user is found for the following entries
		than it returns null
	*/
	public static User Login (String username, String password) {
		try {
			HttpWebResponse response = SendGETRequest (DBServerAddr + "/users?username="+username+"&password="+Encrypt(password));

			if (response.StatusCode != HttpStatusCode.OK) {
				return null;
			} else {
				return JsonUtility.FromJson<User> (GetMessage (response));
			}
		} catch (Exception e) {
			Debug.LogError (e);
			return null;
		}
	}

	/* Issues logout request to the DB server */
	public static bool Logout () {
		return true;
	}

	/* Issues register request to DB server */
	public static User Register (String username, String password, String email) {
		User user = new User (username, Encrypt (password), email);

		try {
			HttpWebResponse response = SendPOSTRequest (DBServerAddr + "/register", JsonUtility.ToJson (user));

			if (response.StatusCode != HttpStatusCode.OK) {
				return null;
			}

			return user;
		} catch (Exception e) {
			Debug.LogError (e);
			return null;
		}
	}

	private static String Encrypt(String message) {
		//byte[] encryptedPassBytes = SHA1.Create ().ComputeHash (Encoding.Unicode.GetBytes(message));
		//return Encoding.Unicode.GetString (encryptedPassBytes);
		return message
	}

	/* Gets the string message of the response */
	private static String GetMessage (HttpWebResponse response) {
		Stream responseData = response.GetResponseStream ();
		StreamReader sr = new StreamReader (responseData);
		String message = sr.ReadToEnd ();
		sr.Close ();
		response.Close ();
		return message;
	}

	/* Generates a default template for a request which includes the headers */
	private static HttpWebRequest GetRequestTemplate (String addr, String method) {
		HttpWebRequest request = (HttpWebRequest) WebRequest.Create(addr);
		request.UserAgent = "Imperial Dungeon";
		request.Credentials = CredentialCache.DefaultCredentials;
		request.ContentType = "application/x-www-form-urlencoded";
		request.Method = method;

		return request;
	}

	/* Creates a GET request and sends it to the DB server */
	private static HttpWebResponse SendGETRequest (String addr) {
		HttpWebRequest request = GetRequestTemplate (addr, "GET");

		return (HttpWebResponse) request.GetResponse ();
	}

	/* Creates a POST request and sends it to the DB server */
	private static HttpWebResponse SendPOSTRequest (String addr, String message) {
		HttpWebRequest request = GetRequestTemplate (addr, "POST");

		Byte[] bytes = Encoding.UTF8.GetBytes (message);
		request.ContentLength = bytes.Length;
		Stream dataStream = request.GetRequestStream ();
		dataStream.Write (bytes, 0, bytes.Length);
		dataStream.Close ();
		return (HttpWebResponse) request.GetResponse ();
	}
}