using System;
using System.Net;

public class Response<T> {
	public T data;
	public HttpStatusCode status;
	public String error;

	public Response (T data, HttpStatusCode status) {
		this.data = data;
		this.status = status;
	}

	public Response (T data, String error) {
		this.data = data;
		this.error = error;
	}
}

