using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex {

	public Vector2 point;
	private List<Vertex> connections = new List<Vertex> ();

	public Vertex (float p1, float p2) {
		this.point = new Vector2 (p1, p2);
	}

	public Vertex (Vector2 point) {
		this.point = point;
	}

	public override string ToString () {
		return point.ToString () + " conns: " + connections.Count;
	}

	public override int GetHashCode () {
		return base.GetHashCode ();
	}

	public void AddConnection (Vertex v) {
		connections.Add (v);
	}

	public bool ConnContains (Vertex v) {
		return connections.Contains (v);
	}

	public List<Vertex> GetConnections () {
		return connections;
	}

	public override bool Equals (object obj) {
		if (!(obj is Vertex)) {
			return false;
		}

		Vertex other = (Vertex) obj;

		return other.point.Equals (point);
	}
}
