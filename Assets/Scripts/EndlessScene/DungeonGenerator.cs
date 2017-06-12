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

	// Use this for initialization
	void Start () {
		StartCoroutine (GenerateInitialRooms ());
	}

	private IEnumerator GenerateInitialRooms () {
		// startup time
		yield return new WaitForSeconds (0.5f);
		for (int i = 0; i < numberOfInitialGeneratedRooms; i++) {
			GenerateRoom ();
			yield return new WaitForSeconds (0.01f);
		}
	}

	public Vector2 GetRandomPointInEllipse () {
		float t = 2 * Mathf.PI * Random.value;
		float u = Random.value + Random.value;
		float r = u > 1 ? 2 - u : u;

		return new Vector2 (ellipseSize.x * r * Mathf.Cos (t) / 2, ellipseSize.y * r * Mathf.Sin (t) / 2);
	}

	public void GenerateRoom () {
		Vector2 position = GetRandomPointInEllipse ();
		GameObject room = Instantiate (roomPrefab);
		room.GetComponent <RoomSetup> ().Init (position, Random.Range(minRoomWidth, maxRoomWidth), Random.Range (minRoomHeight, maxRoomHeight), transform);
	}
}
