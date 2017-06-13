using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex {

	public Vector2 point;
	public List<Vertex> connectingPoints;

	public Vertex (Vector2 point) {
		this.point = point;
		this.connectingPoints = new List<Vertex> ();
	}

	public override string ToString () {
		return point.ToString () + " - " + connectingPoints.ToArray ().ToStringFull ();
	}

	public override int GetHashCode () {
		return base.GetHashCode ();
	}

	public override bool Equals (object obj) {
		if (!(obj is Vertex)) {
			return false;
		}

		Vertex other = (Vertex) obj;

		return other.point == point;
	}
}
