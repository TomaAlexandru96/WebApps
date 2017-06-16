using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : MonoBehaviour {
	
	public Room r1;
	public Room r2;
	public GameObject tilePrefab;
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
		Vector2 point3 = new Vector2 (0, 0);
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

			CreateHallway (point1, point2);
		} else {
			// L shape
			if (sizeR1.center.x > sizeR2.center.x && sizeR1.center.y > sizeR2.center.y) {
				if (Mathf.Abs (delta.x) < Mathf.Abs (delta.y)) {
					point1.x = sizeR1.center.x;
					point1.y = sizeR1.yMin;
					point3.x = sizeR2.xMax;
					point3.y = sizeR2.center.y;
					point2.x = point1.x;
					point2.y = point3.y;
				} else {
					point1.x = sizeR1.xMin;
					point1.y = sizeR1.center.y;
					point3.x = sizeR2.center.x;
					point3.y = sizeR2.yMax;
					point2.x = point3.x;
					point2.y = point1.y;
				}
			} else if (sizeR1.center.x > sizeR2.center.x && sizeR1.center.y < sizeR2.center.y) {
				if (Mathf.Abs (delta.x) < Mathf.Abs (delta.y)) {
					point1.x = sizeR1.center.x;
					point1.y = sizeR1.yMax;
					point3.x = sizeR2.xMax;
					point3.y = sizeR2.center.y;
					point2.x = point1.x;
					point2.y = point3.y;
				} else {
					point1.x = sizeR1.xMin;
					point1.y = sizeR1.center.y;
					point3.x = sizeR2.center.x;
					point3.y = sizeR2.yMin;
					point2.x = point3.x;
					point2.y = point1.y;
				}
			} else if (sizeR1.center.x < sizeR2.center.x && sizeR1.center.y < sizeR2.center.y) {
				if (Mathf.Abs (delta.x) < Mathf.Abs (delta.y)) {
					point1.x = sizeR1.center.x;
					point1.y = sizeR1.yMax;
					point3.x = sizeR2.xMin;
					point3.y = sizeR2.center.y;
					point2.x = point1.x;
					point2.y = point3.y;
				} else {
					point1.x = sizeR1.xMax;
					point1.y = sizeR1.center.y;
					point3.x = sizeR2.center.x;
					point3.y = sizeR2.yMin;
					point2.x = point3.x;
					point2.y = point1.y;
				}
			} else {
				if (Mathf.Abs (delta.x) < Mathf.Abs (delta.y)) {
					point1.x = sizeR1.center.x;
					point1.y = sizeR1.yMin;
					point3.x = sizeR2.xMin;
					point3.y = sizeR2.center.y;
					point2.x = point1.x;
					point2.y = point3.y;
				} else {
					point1.x = sizeR1.xMax;
					point1.y = sizeR1.center.y;
					point3.x = sizeR2.center.x;
					point3.y = sizeR2.yMax;
					point2.x = point3.x;
					point2.y = point1.y;
				}
			}
			CreateHallway (point1, point2, point3);
		}
	}

	public bool IsHallway2 () {
		return points.Count == 2;
	}

	public void CreateHallway (params Vector2[] ls) {
		foreach (var p in ls) {
			points.Add (p);
		}

		for (int i = 1; i < points.Count; i++) {
			Vector2 p1 = new Vector2 (points [i - 1].x, points [i - 1].y);
			Vector2 p2 = new Vector2 (points [i].x, points [i].y);

			GameObject tile1 = Instantiate (tilePrefab, new Vector3 (p1.x, p1.y, 0), Quaternion.identity);
			GameObject tile2 = Instantiate (tilePrefab, new Vector3 (p2.x, p2.y, 0), Quaternion.identity);
			tile1.GetComponent<SpriteRenderer> ().sortingOrder = -2;
			tile2.GetComponent<SpriteRenderer> ().sortingOrder = -2;
		}

		SetColor (Color.blue);
	}

	public void SetColor (Color color) {
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).GetComponent<SpriteRenderer> ().color = color;
		}
	}
	
}
