using System;

public class Response {
	private String message;
	private Status status;

	public Response (String message, Status status) {
		this.message = message;
		this.status = status;
	}

	public Response (Status status) {
		this.message = "";
		this.status = status;
	}

	public String GetMessage () {
		return message;
	}

	public Status GetStatus () {
		return status;
	}

	public override string ToString ()
	{
		return "Message: " + message + " Status: " + status;
	}
}

