using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph {

	private List<Vertex> nodes;

	public Graph (List<Vertex> nodes) {
		this.nodes = nodes;
	}

	public void ApplyDFS () {
		// TODO
	}

	public List<Vertex> GetNodes () {
		return nodes;
	}

	public override string ToString () {
		string res = nodes.Count + " ";

		foreach (var node in nodes) {
			res += node.ToString () + "\n";
		}

		return res;
	}
}
