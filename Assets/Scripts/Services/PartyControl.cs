using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyControl : MonoBehaviour {

	public GameObject player;
	public GameObject party;

	public void addPlayer() {
		GameObject player2 = Instantiate (player, party.transform);
		player2.GetComponent<RectTransform> ().position = new Vector3 (0, 0, 0);
//		GameObject player2 = new GameObject("Image");
//		player2.AddComponent<RectTransform> ();
//		player2.AddComponent<CanvasRenderer> ();
//		Vector3 position = new Vector3(player.transform.position);
//		player2.transform.parent = party.transform;
	} 
}
