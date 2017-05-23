using System;
using System.Net;

public class Response<T> {
	public T data = default(T);
	public WebException error = null;

	public Response (T data) {
		this.data = data;
	}

	public Response (WebException error) {
		this.error = error;
	}
}

