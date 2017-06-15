using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : MonoBehaviour {

	private Room r1;
	private Room r2;

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

		bool createHallway2 = false;
		Vector2 point1 = new Vector2 (0, 0);
		Vector2 point2 = new Vector2 (0, 0);

		if (sizeR1.center.y > sizeR2.center.y) {
			point1.y = sizeR1.yMin;
			point2.y = sizeR2.yMax;

			if (deltaRight > 0) {
				point1.x = deltaRight / 2;
				point2.x = deltaRight / 2;
				createHallway2 = true;
			} else if (deltaLeft > 0) {
				point1.x = deltaLeft / 2;
				point2.x = deltaLeft / 2;
				createHallway2 = true;
			}
		} else if (sizeR1.center.x > sizeR2.center.x) {
			point1.x = sizeR1.xMin;
			point2.x = sizeR2.xMax;

			if (deltaBottom > 0) {
				point1.y = deltaBottom / 2;
				point2.y = deltaBottom / 2;
				createHallway2 = true;
			} else if (deltaTop > 0) {
				point1.y = deltaTop / 2;
				point2.y = deltaTop / 2;
				createHallway2 = true;
			}
		} else if (sizeR1.center.y < sizeR2.center.y) {
			point1.y = sizeR1.yMax;
			point2.y = sizeR2.yMin;

			if (deltaRight > 0) {
				point1.x = deltaRight / 2;
				point2.x = deltaRight / 2;
				createHallway2 = true;
			} else if (deltaLeft > 0) {
				point1.x = deltaLeft / 2;
				point2.x = deltaLeft / 2;
				createHallway2 = true;
			}
		} else if (sizeR1.center.x < sizeR2.center.x) {
			point1.x = sizeR1.xMax;
			point2.x = sizeR2.xMin;

			if (deltaBottom > 0) {
				point1.y = deltaBottom / 2;
				point2.y = deltaBottom / 2;
				createHallway2 = true;
			} else if (deltaTop > 0) {
				point1.y = deltaTop / 2;
				point2.y = deltaTop / 2;
				createHallway2 = true;
			}
		}

		if (createHallway2) {
			CreateHallway2 (point1, point2);
			return;
		}
	}

	public void CreateHallway2 (Vector2 point1, Vector2 point2) {
		Debug.Log (point1 + " connected to " + point2);
	}

	public void CreateHallway3 (Vector2 point1, Vector2 point2) {
	}
}
