using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : MonoBehaviour {
	
	public Room r1;
	public Room r2;
	public bool isHallway2 = false;
	public bool isHallway3 = false;
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

		float deltaRight = sizeR1.xMax - sizeR2.xMin;
		float deltaLeft = sizeR2.xMax - sizeR1.xMin;
		float deltaBottom = sizeR2.yMax - sizeR1.yMin;
		float deltaTop = sizeR1.yMax - sizeR2.yMin;

		// check if they are close
		// r2 -> bottom, left, top, right
		// not overlapping

		Vector2 point1 = new Vector2 (0, 0);
		Vector2 point2 = new Vector2 (0, 0);

		if (sizeR1.center.y > sizeR2.center.y && (deltaRight > 0 || deltaLeft > 0)) {
			point1.y = sizeR1.yMin;
			point2.y = sizeR2.yMax;

			if (deltaRight > 0) {
				point1.x = sizeR1.center.x + deltaRight / 2;
				point2.x = sizeR1.center.x + deltaRight / 2;
				Debug.Log (sizeR1 + " " + sizeR2);
				Debug.Log (point1 + " " + point2);
				Debug.Log ("Bottom deltaRight");
				isHallway2 = true;
			} else if (deltaLeft > 0) {
				point1.x = sizeR1.center.x - deltaLeft / 2;
				point2.x = sizeR1.center.x - deltaLeft / 2;
				Debug.Log (sizeR1 + " " + sizeR2);
				Debug.Log (point1 + " " + point2);
				Debug.Log ("Bottom deltaLeft");
				isHallway2 = true;
			}
		} else if (sizeR1.center.x > sizeR2.center.x && (deltaBottom > 0 || deltaTop > 0)) {
			point1.x = sizeR1.xMin;
			point2.x = sizeR2.xMax;

			if (deltaBottom > 0) {
				point1.y = sizeR1.center.y - deltaBottom / 2;
				point2.y = sizeR1.center.y - deltaBottom / 2;
				isHallway2 = true;
				Debug.Log (sizeR1 + " " + sizeR2);
				Debug.Log (point1 + " " + point2);
				Debug.Log ("Left deltaBottom");
			} else if (deltaTop > 0) {
				point1.y = sizeR1.center.y + deltaTop / 2;
				point2.y = sizeR1.center.y + deltaTop / 2;
				Debug.Log (sizeR1 + " " + sizeR2);
				Debug.Log (point1 + " " + point2);
				Debug.Log ("Left deltaTop");
				isHallway2 = true;
			}
		} else if (sizeR1.center.y < sizeR2.center.y && (deltaRight > 0 || deltaLeft > 0)) {
			point1.y = sizeR1.yMax;
			point2.y = sizeR2.yMin;

			if (deltaRight > 0) {
				point1.x = sizeR1.center.x + deltaRight / 2;
				point2.x = sizeR1.center.x + deltaRight / 2;
				isHallway2 = true;
				Debug.Log (sizeR1 + " " + sizeR2);
				Debug.Log (point1 + " " + point2);
				Debug.Log ("Top deltaRight");
			} else if (deltaLeft > 0) {
				point1.x = sizeR1.center.x - deltaLeft / 2;
				point2.x = sizeR1.center.x - deltaLeft / 2;
				isHallway2 = true;
				Debug.Log (sizeR1 + " " + sizeR2);
				Debug.Log (point1 + " " + point2);
				Debug.Log ("Top deltaLeft");
			}
		} else if (sizeR1.center.x < sizeR2.center.x && (deltaBottom > 0 || deltaTop > 0)) {
			point1.x = sizeR1.xMax;
			point2.x = sizeR2.xMin;

			if (deltaBottom > 0) {
				point1.y = sizeR1.center.y - deltaBottom / 2;
				point2.y = sizeR1.center.y - deltaBottom / 2;
				isHallway2 = true;
				Debug.Log (sizeR1 + " " + sizeR2);
				Debug.Log (point1 + " " + point2);
				Debug.Log ("Right deltaBottom");
			} else if (deltaTop > 0) {
				point1.y = sizeR1.center.y + deltaTop / 2;
				point2.y = sizeR1.center.y + deltaTop / 2;
				isHallway2 = true;
				Debug.Log (sizeR1 + " " + sizeR2);
				Debug.Log (point1 + " " + point2);
				Debug.Log ("Right deltaTop");
			}
		}

		if (isHallway2) {
			CreateHallway2 (point1, point2);
			return;
		}
	}

	public void CreateHallway2 (Vector2 point1, Vector2 point2) {
		points.Add (point1);
		points.Add (point2);
	}

	public void CreateHallway3 (Vector2 point1, Vector2 point2) {
	}
}
