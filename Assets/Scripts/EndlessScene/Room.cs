using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour {

	public float outlineBorderSize;
	public GameObject tilePrefab;
	public GameObject outline;
	public GameObject nodePrefab;
	public List<Room> MSTList = new List<Room> ();
	public List<Room> DelaunayList = new List<Room> ();

	private GameObject node;

	public void Init (Vector2 position, int width, int height, Transform parent) {
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

				pos.x = tile.GetComponent<SpriteRenderer> ().size.x * x;
				pos.y = tile.GetComponent<SpriteRenderer> ().size.y * y;

				tile.transform.localPosition = pos;
			}
		}

		Vector3 scale = outline.transform.localScale;
		scale.x = width + outlineBorderSize;
		scale.y = height + outlineBorderSize;
		outline.transform.localScale = scale;

		Vector3 outlinePos = outline.transform.localPosition;
		outlinePos.x = tilePrefab.GetComponent<SpriteRenderer> ().size.x * width / 2 - tilePrefab.GetComponent<SpriteRenderer> ().size.x / 2;
		outlinePos.y = tilePrefab.GetComponent<SpriteRenderer> ().size.y * height / 2 - tilePrefab.GetComponent<SpriteRenderer> ().size.y / 2;
		outline.transform.localPosition = outlinePos;

		Vector3 colliderSize = GetComponent<BoxCollider2D> ().size;
		colliderSize.x *= width;
		colliderSize.y *= height;
		GetComponent<BoxCollider2D> ().size = colliderSize;

		Vector2 colliderOffset = GetComponent<BoxCollider2D> ().offset;
		colliderOffset.x = outlinePos.x;
		colliderOffset.y = outlinePos.y;
		GetComponent<BoxCollider2D> ().offset = colliderOffset;

		// instantiate center node
		node = Instantiate (nodePrefab);
		node.transform.position = outlinePos;
		node.transform.SetParent (transform, false);
		node.transform.localScale *= 2;
		SetNode (false);
	}

	public void SetColor (Color color) {
		for (int i = 1; i < transform.childCount - 1; i++) {
			transform.GetChild (i).GetComponent<SpriteRenderer> ().color = color;
		}
	}

	public Vector3 GetPosition () {
		return node.transform.position;
	}

	public void SetNode (bool active) {
		node.SetActive (active);
	}

	public Vector3 GetSize () {
		return outline.transform.localScale;
	}
}
