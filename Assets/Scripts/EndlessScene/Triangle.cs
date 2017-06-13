using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle {

	public Vertex a;
	public Vertex b;
	public Vertex c;

	public Triangle (Vertex a, Vertex b, Vertex c) {
		
		if (IsCounterClockwise (a, b, c)) {
			this.a = a;
			this.b = b;
			this.c = c;
		} else {
			this.a = a;
			this.b = c;
			this.c = b;
		}
	}

	public bool IsPointInCircle (Vertex d) {
		float[,] m = new float[,] {
			{a.point.x, a.point.y, a.point.x * a.point.x + a.point.y * a.point.y, 1},
			{b.point.x, b.point.y, b.point.x * b.point.x + b.point.y * b.point.y, 1},
			{c.point.x, c.point.y, c.point.x * c.point.x + c.point.y * c.point.y, 1},
			{d.point.x, d.point.y, d.point.x * d.point.x + d.point.y * d.point.y, 1}
		};

		return
			(m[0, 3] * m[1, 2] * m[2, 1] * m[3, 0] - m[0, 2] * m[1, 3] * m[2, 1] * m[3, 0] -
			m[0, 3] * m[1, 1] * m[2, 2] * m[3, 0] + m[0, 1] * m[1, 3] * m[2, 2] * m[3, 0] +
			m[0, 2] * m[1, 1] * m[2, 3] * m[3, 0] - m[0, 1] * m[1, 2] * m[2, 3] * m[3, 0] -
			m[0, 3] * m[1, 2] * m[2, 0] * m[3, 1] + m[0, 2] * m[1, 3] * m[2, 0] * m[3, 1] +
			m[0, 3] * m[1, 0] * m[2, 2] * m[3, 1] - m[0, 0] * m[1, 3] * m[2, 2] * m[3, 1] -
			m[0, 2] * m[1, 0] * m[2, 3] * m[3, 1] + m[0, 0] * m[1, 2] * m[2, 3] * m[3, 1] +
			m[0, 3] * m[1, 1] * m[2, 0] * m[3, 2] - m[0, 1] * m[1, 3] * m[2, 0] * m[3, 2] -
			m[0, 3] * m[1, 0] * m[2, 1] * m[3, 2] + m[0, 0] * m[1, 3] * m[2, 1] * m[3, 2] +
			m[0, 1] * m[1, 0] * m[2, 3] * m[3, 2] - m[0, 0] * m[1, 1] * m[2, 3] * m[3, 2] -
			m[0, 2] * m[1, 1] * m[2, 0] * m[3, 3] + m[0, 1] * m[1, 2] * m[2, 0] * m[3, 3] +
			m[0, 2] * m[1, 0] * m[2, 1] * m[3, 3] - m[0, 0] * m[1, 2] * m[2, 1] * m[3, 3] -
			m[0, 1] * m[1, 0] * m[2, 2] * m[3, 3] + m[0, 0] * m[1, 1] * m[2, 2] * m[3, 3]) > 0;
	}

	public bool IsCounterClockwise (Vertex a, Vertex b, Vertex c) {
		return (b.point.x - a.point.x) * (c.point.y - a.point.y) - (c.point.x - a.point.x) * (b.point.y - a.point.y) > 0;
	}

	public void ValidateEdge (Vertex p, DelauneyTriangulation dt) {
		List<Triangle> triangles = dt.GetDT ();
		// find triangle adj to this triangle
		Edge commonEdge = FindOpEdge (p);
		Triangle adjTriangle = null;
		foreach (var t in triangles) {
			if (t.ContainsEdge (commonEdge) && !t.Equals (this)) {
				adjTriangle = t;
				break;
			}
		}

		if (adjTriangle == null) {
			return;
		}

		// flip edge if needed
		if (adjTriangle.IsPointInCircle (p)) {
			KeyValuePair<Triangle, Triangle> res = Triangle.FilpEdge (this, adjTriangle, commonEdge, dt);
			res.Key.ValidateEdge (p, dt);
			res.Value.ValidateEdge (p, dt);
		}
	}

	public static KeyValuePair<Triangle, Triangle> FilpEdge (Triangle t1, Triangle t2, Edge commonEdge, DelauneyTriangulation dt) {
		List<Triangle> triangles = dt.GetDT ();

		Vertex newP1 = t1.FindOpPoint (commonEdge);
		Vertex newP2 = t2.FindOpPoint (commonEdge);

		Triangle newT1 = new Triangle (newP1, commonEdge.p1, newP2);
		Triangle newT2 = new Triangle (newP1, commonEdge.p2, newP2);

		triangles.Remove (t1);
		triangles.Remove (t2);
		triangles.Add (newT1);
		triangles.Add (newT2);

		return new KeyValuePair<Triangle, Triangle> (newT1, newT2);
	}

	private bool ContainsEdge (Edge e) {
		bool p1Found = false;
		bool p2Found = false;

		p1Found = e.p1.Equals (a) || e.p1.Equals (b) || e.p1.Equals (c);
		p2Found = e.p2.Equals (a) || e.p2.Equals (b) || e.p2.Equals (c);

		return p1Found && p2Found;
	}

	private Vertex FindOpPoint (Edge e) {
		if (e.Equals (FindOpEdge (a))) {
			return a;
		} else if (e.Equals (FindOpEdge (b))) {
			return b;
		} else if (e.Equals (FindOpEdge (c))) {
			return c;
		} else {
			Debug.LogError ("Edge " + e.ToString () + " does not belong to traignle " + ToString ());
			return null;
		}
	}

	public Edge FindOpEdge (Vertex p) {
		if (p.Equals (a)) {
			return new Edge (b, c);
		} else if (p.Equals (b)) {
			return new Edge (a, c);
		} else if (p.Equals (c)) {
			return new Edge (a, b);
		} else {
			Debug.LogError ("Vertex " + p.ToString () + " does not belon to the triangle " + ToString ());
			return null;
		}
	}

	public override string ToString () {
		return "a: " + a.ToString () + ", b: " + b.ToString () + ", c: " + c.ToString ();
	}

	public override int GetHashCode () {
		return base.GetHashCode ();
	}

	public override bool Equals (object obj) {
		if (!(obj is Triangle)) {
			return false;
		}

		Triangle other = (Triangle) obj;
		return a.Equals (other.a) && b.Equals (other.b) && c.Equals (other.c) ||
			a.Equals (other.a) && b.Equals (other.c) && c.Equals (other.b) ||
			a.Equals (other.b) && b.Equals (other.a) && c.Equals (other.c) ||
			a.Equals (other.b) && b.Equals (other.c) && c.Equals (other.a) ||
			a.Equals (other.c) && b.Equals (other.a) && c.Equals (other.b) ||
			a.Equals (other.c) && b.Equals (other.b) && c.Equals (other.a);
	}

	public bool IsPointInTriangle (Vertex pt) {
		bool b1, b2, b3;

		b1 = !IsCounterClockwise (pt, a, b);
		b2 = !IsCounterClockwise (pt, b, c);
		b3 = !IsCounterClockwise (pt, c, a);

		return ((b1 == b2) && (b2 == b3));
	}
}
