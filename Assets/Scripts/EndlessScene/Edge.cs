using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge {

	public Vertex p1;
	public Vertex p2;

	public Edge (Vertex p1, Vertex p2) {
		this.p1 = p1;
		this.p2 = p2;
	}

	public override string ToString () {
		return p1 + ", " + p2;
	}

	public override int GetHashCode () {
		return base.GetHashCode ();
	}

	public override bool Equals (object obj) {
		if (!(obj is Edge)) {
			return false;
		}

		Edge other = (Edge) obj;

		return p1.Equals (other.p1) && p2.Equals (other.p2)
			|| p1.Equals (other.p2) && p2.Equals (other.p1);
	}
}
