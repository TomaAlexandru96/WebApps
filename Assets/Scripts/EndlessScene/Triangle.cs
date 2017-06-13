using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle {

	public Vertex a;
	public Vertex b;
	public Vertex c;

	public Triangle (Vertex a, Vertex b, Vertex c) {
		
		if (!IsCounterClockwise (a, b, c)) {
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

	public void ValidateEdge (Edge e) {
		
	}
}
