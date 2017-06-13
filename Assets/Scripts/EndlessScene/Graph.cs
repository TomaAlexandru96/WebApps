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

	private void AddEdge (Vertex v1, Vertex v2) {
		graph [v1].Add (v2);
		graph [v2].Add (v1);
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
					AddEdge (p, opEdge.p1);
				}

				if (!graph [p].Contains (opEdge.p2)) {
					AddEdge (p, opEdge.p2);
				}
			}
		}
	}
}
