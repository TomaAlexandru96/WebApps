using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelauneyTriangulation {

	private List<Vertex> points;
	private List<Triangle> dt = new List<Triangle> ();

	public DelauneyTriangulation (List<Vertex> points) {
		this.points = points;
	}

	public Graph Apply () {
		const float max = 150f;
		Vertex a = new Vertex (-max, -max);
		Vertex b = new Vertex (max, -max);
		Vertex c = new Vertex (0, max);
		Triangle dummy = new Triangle (a, b, c);

		dt.Add (dummy);

		foreach (var point in points) {
			Triangle main = null;

			foreach (var t in dt) {
				if (t.IsPointInTriangle (point)) {
					main = t;
					break;
				}
			}

			Triangle t1 = new Triangle (point, main.a, main.b);
			Triangle t2 = new Triangle (point, main.b, main.c);
			Triangle t3 = new Triangle (point, main.a, main.c);

			dt.Remove (main);
			dt.Add (t1);
			dt.Add (t2);
			dt.Add (t3);

			t1.ValidateEdge (point, this);
			t2.ValidateEdge (point, this);
			t3.ValidateEdge (point, this);
		}

		List<Triangle> good = new List<Triangle> ();

		dt.ForEach ((triangle) => {
			if (a.Equals (triangle.a) || a.Equals (triangle.b) || a.Equals (triangle.c)) {
				return;
			}

			if (b.Equals (triangle.a) || b.Equals (triangle.b) || b.Equals (triangle.c)) {
				return;
			}

			if (c.Equals (triangle.a) || c.Equals (triangle.b) || c.Equals (triangle.c)) {
				return;
			}

			good.Add (triangle);
		});

		dt = good;
			

		return GetGraph ();
	}

	private Graph GetGraph () {
		List<Vertex> nodes = new List<Vertex> ();

		foreach (Triangle t in dt) {
			Vertex[] ps = new Vertex[] {t.a, t.b, t.c};
			foreach (var p in ps) {
				if (!nodes.Contains (p)) {
					nodes.Add (p);
				}
			}

			foreach (var p in ps) {
				Vertex gn = nodes.Find ((vertex)=> {return vertex.Equals (p);});
				Edge opEdge = t.FindOpEdge (p);

				Vertex v1 = nodes.Find ((vertex) => {return vertex.Equals (opEdge.p1);});
				Vertex v2 = nodes.Find ((vertex) => {return vertex.Equals (opEdge.p2);});

				if (!gn.ConnContains (v1)) {
					gn.AddConnection (v1);
				}
					
				if (!gn.ConnContains (v2)) {
					gn.AddConnection (v2);
				}
			}
		}

		return new Graph (nodes);
	}

	public List<Triangle> GetDT () {
		return dt;
	}
}
