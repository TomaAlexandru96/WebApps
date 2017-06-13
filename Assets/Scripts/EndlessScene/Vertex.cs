using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex {

	public Vector2 point;

	public Vertex (float p1, float p2) {
		this.point = new Vector2 (p1, p2);
	}

	public Vertex (Vector2 point) {
		this.point = point;
	}

	public override string ToString () {
		return point.ToString ();
	}

	public override int GetHashCode () {
		return base.GetHashCode ();
	}

	public override bool Equals (object obj) {
		if (!(obj is Vertex)) {
			return false;
		}

		Vertex other = (Vertex) obj;

		return other.point.Equals (point);
	}

	public static float GetDistance (Vertex a, Vertex b) {
		return Vector2.Distance (a.point, b.point);
	}
}
