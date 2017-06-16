using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : MonoBehaviour {
	
	public Room r1;
	public Room r2;
	public List<Vector2> points = new List<Vector2> ();

	public bool Contains (Room r) {
		return r1.GetPosition ().Equals (r.GetPosition ()) || r2.GetPosition ().Equals (r.GetPosition ());
	}

	// the get rect function does not work properly
	public void Init (Room r1, Room r2) {
		this.r1 = r1;
		this.r2 = r2;

		Rect sizeR1 = r1.GetRect ();
		Rect sizeR2 = r2.GetRect ();

		Vector2 point1 = new Vector2 (0, 0);
		Vector2 point2 = new Vector2 (0, 0);
		Vector2 delta = sizeR2.center - sizeR1.center;

		if ((Mathf.Abs (delta.x) < sizeR1.width / 2 + sizeR2.width / 2) 
					|| (Mathf.Abs (delta.y) < sizeR1.height / 2 + sizeR2.height / 2)) {

			if (sizeR1.center.y > sizeR2.center.y && (Mathf.Abs (delta.y) >= sizeR1.height / 2 + sizeR2.height / 2)) {
				// bottom
				point1.x = sizeR1.center.x + delta.x / 2;
				point2.x = sizeR1.center.x + delta.x / 2;
				point1.y = sizeR1.yMin;
				point2.y = sizeR2.yMax;
			} else if (sizeR1.center.x > sizeR2.center.x && (Mathf.Abs (delta.x) >= sizeR1.width / 2 + sizeR2.width / 2)) {
				// left
				point1.y = sizeR1.center.y + delta.y / 2;
				point2.y = sizeR1.center.y + delta.y / 2;
				point1.x = sizeR1.xMin;
				point2.x = sizeR2.xMax;
			} else if (sizeR1.center.y < sizeR2.center.y && (Mathf.Abs (delta.y) >= sizeR1.height / 2 + sizeR2.height / 2)) {
				// top
				point1.x = sizeR1.center.x + delta.x / 2;
				point2.x = sizeR1.center.x + delta.x / 2;
				point1.y = sizeR1.yMax;
				point2.y = sizeR2.yMin;
			} else {
				// right
				point1.y = sizeR1.center.y + delta.y / 2;
				point2.y = sizeR1.center.y + delta.y / 2;
				point1.x = sizeR1.xMax;
				point2.x = sizeR2.xMin;
			}

			CreateHallway2 (point1, point2);
		}
	}

	public bool IsHallway2 () {
		return points.Count == 2;
	}

	public void CreateHallway2 (Vector2 point1, Vector2 point2) {
		points.Add (point1);
		points.Add (point2);
	}

	public void CreateHallway3 (Vector2 point1, Vector2 point2) {
	}
}
