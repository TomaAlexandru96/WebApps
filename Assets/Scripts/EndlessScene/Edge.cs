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

	public bool IsRightOf (Vertex x) {
		// TODO
		return false;
	}
		
}
