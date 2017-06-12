using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * All credits for this algorithm are given to 
 * http://www.gamasutra.com/blogs/AAdonaac/20150903/252889/Procedural_Dungeon_Generation_Algorithm.php
*/
public class DungeonGenerator : MonoBehaviour {

	public Vector2 ellipseSize;
	public int maxRoomWidth;
	public int maxRoomHeight;
	public int minRoomWidth;
	public int minRoomHeight;
	public int numberOfInitialGeneratedRooms;
	public int numberOfRooms;
	public GameObject roomPrefab;
	public GameObject linePrefab;

	private List<Room> rooms = new List<Room> ();
	private List<Room> mainRooms = new List<Room> ();

	// Use this for initialization
	void Start () {
		StartCoroutine (GenerateInitialRooms ());
	}

	private void StartSecondStep () {
		SelectMainRooms ();
		DoDelaunayTriangulation (mainRooms);
		DoMST (mainRooms);

		foreach (var room in mainRooms) {
			room.SetNode (true);
			room.SetColor (Color.red);
		}

		List<GameObject> lines = new List<GameObject> ();
		foreach (var room in mainRooms) {
			foreach (var other in room.DelaunayList) {
				lines.Add (CreateLine (room.GetPosition (), other.GetPosition ()));
			}
		}
	}

	private void SelectMainRooms () {
		Vector2 averageSize = new Vector2 (0f, 0f);

		foreach (var room in rooms) {
			averageSize.x += room.GetSize ().x;
			averageSize.y += room.GetSize ().y;
		}

		averageSize.x /= rooms.Count;
		averageSize.y /= rooms.Count;

		foreach (var room in rooms) {
			if (room.GetSize ().x > averageSize.x * 1.10f && room.GetSize ().y > averageSize.y * 1.10f) {
				mainRooms.Add (room);
			}
		}
	}

	private GameObject CreateLine (Vector3 position1, Vector3 position2) {
		GameObject line = Instantiate (linePrefab);
		line.GetComponent<LineRenderer> ().SetPosition (0, position1);
		line.GetComponent<LineRenderer> ().SetPosition (1, position2);
		line.GetComponent<LineRenderer> ().sortingOrder = 10;
		line.GetComponent<LineRenderer> ().startWidth = 0.10f;
		line.GetComponent<LineRenderer> ().endWidth = 0.10f;
		line.transform.SetParent (transform, false);
		return line;
	}

	private void DoDelaunayTriangulation (List<Room> rooms) {
		
	}

	private void DoMST (List<Room> rooms) {
		
	}

	private IEnumerator GenerateInitialRooms () {
		// startup time
		yield return new WaitForSeconds (0.5f);
		for (int i = 0; i < numberOfInitialGeneratedRooms; i++) {
			rooms.Add (GenerateRoom ());
			yield return new WaitForSeconds (0.01f);
		}
		// wait for collision to finnish
		yield return new WaitForSeconds (1.5f);

		StartSecondStep ();
	}

	public Vector2 GetRandomPointInEllipse () {
		float t = 2 * Mathf.PI * Random.value;
		float u = Random.value + Random.value;
		float r = u > 1 ? 2 - u : u;

		return new Vector2 (ellipseSize.x * r * Mathf.Cos (t) / 2, ellipseSize.y * r * Mathf.Sin (t) / 2);
	}

	public Room GenerateRoom () {
		Vector2 position = GetRandomPointInEllipse ();
		GameObject room = Instantiate (roomPrefab);
		room.GetComponent <Room> ().Init (position, Random.Range(minRoomWidth, maxRoomWidth), Random.Range (minRoomHeight, maxRoomHeight), transform);
		return room.GetComponent <Room> ();
	}
}
