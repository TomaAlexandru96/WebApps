using System;
using System.Net;
using System.Text;
using UnityEngine;
using System.IO;

public class DBServer {

	public const String DBServerAddr = "http://146.169.46.104:8081";

	private DBServer () {
	}

	// TODO
	public static Response login (String username, String password) {
		return DBServer.sendRequest ("login " + username + " " + password, "GET");
	}

	// TODO
	public static Response logout () {
		return DBServer.sendRequest ("logout", "GET");
	}

	private static Response sendRequest (String data, String method) {
		Byte[] bytes = Encoding.UTF8.GetBytes (data);

		// setup template
		WebRequest request = WebRequest.Create (DBServerAddr);
		request.Credentials = CredentialCache.DefaultCredentials;
		((HttpWebRequest)request).UserAgent = "Imperial Dungeon";
		request.ContentLength = bytes.Length;
		request.ContentType = "application/x-www-form-urlencoded";
		request.Method = method;
		Stream dataStream = request.GetRequestStream ();
		dataStream.Write (bytes, 0, bytes.Length);

		// get web response
		WebResponse response = request.GetResponse();

		Stream responseData = response.GetResponseStream ();
		StreamReader sr = new StreamReader (responseData);
		Response resp = new Response (sr.ReadToEnd (), Status.Successful);

		// close streams
		dataStream.Close ();
		sr.Close ();
		responseData.Close ();
		response.Close();

		return resp;
	}

}