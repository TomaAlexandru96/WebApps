using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Graph {

	private Dictionary<Vertex, List<Vertex>> graph = new Dictionary<Vertex, List<Vertex>> ();

	public Graph (DelauneyTriangulation dt) {
		GetGraphFrom (dt);
	}

	// prim s algorithm
	public void ApplyMST () {
		Dictionary<Vertex, List<Vertex>> tree = new Dictionary<Vertex, List<Vertex>> ();
		List<Vertex> visited = new List<Vertex> ();
		Vertex first = null;

		foreach (var entry in graph) {
			if (first == null) {
				first = entry.Key;
			}
			tree.Add (entry.Key, new List<Vertex> ());
		}

		visited.Add (first);

		while (visited.Count < graph.Count) {
			float min = float.MaxValue;
			Edge current = null;

			foreach (var node in visited) {
				foreach (var neighbours in graph[node]) {
					Edge e = new Edge (node, neighbours);
					if (!visited.Contains (neighbours)) {
						if (e.GetDistance () < min) {
							min = e.GetDistance ();
							current = e;
						}
					}
				}
			}

			if (current != null) {
				AddEdge (tree, current.p1, current.p2);
				visited.Add (current.p2);
			}
		}

		graph = tree;
	}

	public void ForEachEdge (Action<Edge> apply) {
		foreach (var node in graph) {
			foreach (var neighbour in node.Value) {
				Edge e = new Edge (node.Key, neighbour);
				apply (e);
			}
		}
	}

	public Dictionary<Vertex, List<Vertex>> GetGraph () {
		return graph;
	}

	public override string ToString () {
		string res = graph.Count + " ";

		ForEachEdge ((e) => {
			res += e + "\n";
		});

		return res;
	}

	public void AddEdge (Dictionary<Vertex, List<Vertex>> g, Vertex v1, Vertex v2) {
		g [v1].Add (v2);
		g [v2].Add (v1);
	}

	private void GetGraphFrom (DelauneyTriangulation dt) {
		foreach (Triangle t in dt.GetDT ()) {
			Vertex[] ps = new Vertex[] {t.a, t.b, t.c};
			foreach (var p in ps) {
				if (!graph.ContainsKey (p)) {
					List<Vertex> neighbours = new List<Vertex> ();
					graph.Add (p, neighbours);
				}
			}

			foreach (var p in ps) {
				Edge opEdge = t.FindOpEdge (p);

				if (!graph [p].Contains (opEdge.p1)) {
					AddEdge (graph, p, opEdge.p1);
				}

				if (!graph [p].Contains (opEdge.p2)) {
					AddEdge (graph, p, opEdge.p2);
				}
			}
		}
	}
}
