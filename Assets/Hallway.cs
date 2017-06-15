using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : MonoBehaviour {

	private Room r1;
	private Room r2;

	public bool Contains (Room r) {
		return r1.GetPosition ().Equals (r.GetPosition ()) || r2.GetPosition ().Equals (r.GetPosition ());
	}

	public void Init (Room r1, Room r2) {
		this.r1 = r1;
		this.r2 = r2;
	}
}
