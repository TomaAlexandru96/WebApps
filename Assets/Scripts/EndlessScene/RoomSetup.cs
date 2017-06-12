using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSetup : MonoBehaviour {

	public float outlineBorderSize;
	public GameObject tilePrefab;
	public GameObject outline;

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

				pos.x = tile.GetComponent<RectTransform> ().rect.width * x;
				pos.y = tile.GetComponent<RectTransform> ().rect.height * y;

				tile.transform.localPosition = pos;
			}
		}

		Vector3 scale = outline.transform.localScale;
		scale.x = width + outlineBorderSize;
		scale.y = height + outlineBorderSize;
		outline.transform.localScale = scale;

		Vector3 outlinePos = outline.transform.localPosition;
		outlinePos.x = tilePrefab.GetComponent<RectTransform> ().rect.width * width / 2 - tilePrefab.GetComponent<RectTransform> ().rect.width / 2;
		outlinePos.y = tilePrefab.GetComponent<RectTransform> ().rect.height * height / 2 - tilePrefab.GetComponent<RectTransform> ().rect.height / 2;
		outline.transform.localPosition = outlinePos;
	}
}
