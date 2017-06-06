using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBag : MonoBehaviour {

	public GameObject itemList;

	public GameObject itemCrown;

	public void Start() {
		AddCrown ();
	}

	public void ShowItem() {
		if (itemList.activeSelf) {
			itemList.SetActive (false);
		} else {
			itemList.SetActive (true);
		}
	}

	public void AddCrown() {
		GameObject items = itemList.transform.GetChild (0).GetChild (0).gameObject;
		GameObject crown = (GameObject) Instantiate (itemCrown);
//		crown.transform.GetComponent<RectTransform> ().localPosition = new Vector3 ()
		crown.transform.SetParent (items.transform, false);
	}
}
