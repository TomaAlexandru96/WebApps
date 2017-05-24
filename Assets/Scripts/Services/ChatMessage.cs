using System;

public class ChatMessage {
	
	public int id;
	public int roomID;
	public int userID;
	public String message;
	public DateTime timestamp;

	public ChatMessage(int roomID, int userID, String message) {
		this.roomID = roomID;
		this.userID = userID;
		this.message = message;
		this.timestamp = new DateTime ();
	}
}

