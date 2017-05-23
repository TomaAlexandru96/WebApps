using System;
using System.Net;
using System.Text;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class DBServer {

	public const String DBServerAddr = "http://146.169.46.104:8081";

	private DBServer () {
	}

	// TODO
	public static Response Login (String username, String password) {
		Dictionary<String, String> headers = new Dictionary<String, String>() {
			{"login", "true"},
			{"username", username},
			{"password", password}
		};

		HttpWebResponse response = SendGETRequest (headers);

		return new Response(GetMessage (response), Status.Successful);
	}

	// TODO
	public static Response Logout () {
		return new Response(Status.Successful);
	}

	private static String GetMessage (HttpWebResponse response) {
		Stream responseData = response.GetResponseStream ();
		StreamReader sr = new StreamReader (responseData);
		String message = sr.ReadToEnd ();
		sr.Close ();
		response.Close ();
		return message;
	}

	private static HttpWebRequest GetRequestTemplate (Dictionary<String, String> headers, String method) {
		HttpWebRequest request = (HttpWebRequest) WebRequest.Create(DBServerAddr);
		request.UserAgent = "Imperial Dungeon";
		request.Credentials = CredentialCache.DefaultCredentials;
		request.ContentType = "application/x-www-form-urlencoded";
		request.Method = method;

		foreach (var header in headers) {
			request.Headers[header.Key] = header.Value;
		}

		return request;
	}

	private static HttpWebResponse SendGETRequest (Dictionary<String, String> headers) {
		HttpWebRequest request = GetRequestTemplate (headers, "GET");

		return (HttpWebResponse) request.GetResponse ();
	}

	private static HttpWebResponse SendPOSTRequest (Dictionary<String, String> headers, String message) {
		HttpWebRequest request = GetRequestTemplate (headers, "POST");

		Byte[] bytes = Encoding.UTF8.GetBytes (message);
		request.ContentLength = bytes.Length;
		Stream dataStream = request.GetRequestStream ();
		dataStream.Write (bytes, 0, bytes.Length);
		dataStream.Close ();
		return (HttpWebResponse) request.GetResponse ();
	}
}