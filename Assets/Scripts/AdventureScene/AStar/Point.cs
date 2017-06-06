using System;

public class Point {
	public int X, Y;

	public Point (int X, int Y) {
		this.X = X;
		this.Y = Y;
	}

	public override bool Equals (object obj) {
		if (!(obj is Point)) {
			return false;
		}

		Point other = (Point) obj;

		return other.X == this.X &&
				other.Y == this.Y;
	}
}

