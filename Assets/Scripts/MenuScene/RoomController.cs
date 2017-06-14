using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviour {

	public MenuController menu;
	public GameObject content;
	public GameObject gamePartyPrefab;
	public Text modeName;

	public void Update () {
		modeName.text = menu.GetMode () == PartyMembers.ADVENTURE ? "Adventure" : "Endless";
	}

	public void Set (bool state) {
		if (state) {
			if (!IsInvoking ("Refresh")) {
				InvokeRepeating ("Refresh", 0f, 1f);
			}
		} else {
			CancelInvoke ();
		}

		gameObject.SetActive (state);
	}

	public void Refresh () {
		NetworkService.GetInstance ().GetRoomList ((matches) => {
			Clear ();
			foreach (var m in matches) {
				GameObject go = Instantiate (gamePartyPrefab);
				go.GetComponent <GamePartyController> ().SetRoomStats (m, menu.GetMode ());
				go.transform.SetParent (content.transform);
			}
		});
	}

	public void Clear () {
		foreach (Transform child in content.transform) {
			Destroy (child.gameObject);
		}
	}
}
