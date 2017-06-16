using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour {

	public GameObject tilePrefab;
	public GameObject nodePrefab;
	public GameObject hallwayPrefab;
	public List<Room> connectingRoom;

	private int height;
	private int width;
	private GameObject node;

	public void Init (Vector2 position, int width, int height, Transform parent) {
		this.height = height;
		this.width = width;
		transform.SetParent (parent);

		Vector3 oldPos = transform.localPosition;
		oldPos.x = position.x;
		oldPos.y = position.y;
		transform.localPosition = oldPos;

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				GameObject tile = Instantiate (tilePrefab);
				tile.transform.SetParent (transform);
				Vector3 pos = tilePrefab.transform.localPosition;

				pos.x = tile.GetComponent<SpriteRenderer> ().size.x * x + tile.GetComponent<SpriteRenderer> ().size.x / 2;
				pos.y = tile.GetComponent<SpriteRenderer> ().size.y * y + tile.GetComponent<SpriteRenderer> ().size.y / 2;

				tile.transform.localPosition = pos;
				tile.GetComponent<SpriteRenderer> ().sortingOrder = -1;
			}
		}

		Vector3 colliderSize = GetComponent<BoxCollider2D> ().size;
		colliderSize.x *= GetRect ().width;
		colliderSize.y *= GetRect ().height;
		GetComponent<BoxCollider2D> ().size = colliderSize;

		Vector3 colliderOffset = GetComponent<BoxCollider2D> ().offset;
		colliderOffset.x += GetRect ().width / 2;
		colliderOffset.y += GetRect ().height / 2;
		GetComponent<BoxCollider2D> ().offset = colliderOffset;

		// instantiate center node
		node = Instantiate (nodePrefab, new Vector3 (GetRect ().width / 2, GetRect ().height / 2), Quaternion.identity);
		node.transform.SetParent (transform, false);
		node.transform.localScale *= 2;
		SetNode (false);
	}

	public void RoundPositionToNearestInt () {
		Vector3 pos = transform.position;
		pos.x = Mathf.Round (pos.x);
		pos.y = Mathf.Round (pos.y);
		transform.position = pos;
	}

	public void SetColor (Color color) {
		for (int i = 0; i < width * height; i++) {
			transform.GetChild (i).GetComponent<SpriteRenderer> ().color = color;
		}
	}

	public void SetNode (bool active) {
		node.SetActive (active);
	}

	public void RemovePhys () {
		GetComponent<Rigidbody2D> ().isKinematic = true;
		GetComponent<BoxCollider2D> ().isTrigger = true;
	}

	public GameObject CreateHallway (Room r2) {
		if (connectingRoom.Contains (r2)) {
			return null;
		}

		connectingRoom.Add (r2);
		r2.connectingRoom.Add (this);

		if (IsIntersecting (r2)) {
			return null;
		}

		GameObject hall = Instantiate (hallwayPrefab);
		hall.GetComponent <Hallway> ().Init (this, r2);
		return hall;
	}

	public bool IsIntersecting (Room other) {
		return GetRect ().Overlaps (other.GetRect ());
	}

	public Vector3 GetPosition () {
		return GetRect ().center;
	}

	public Rect GetRect () {
		Vector2 s = new Vector2 (width * tilePrefab.GetComponent<SpriteRenderer> ().size.x, height * tilePrefab.GetComponent<SpriteRenderer> ().size.y);
		Rect r = new Rect (transform.position, s);
		return r;
	}
}
