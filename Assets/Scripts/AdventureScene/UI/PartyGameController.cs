using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PartyGameController : NetworkBehaviour {

	public GameObject playerPrefab;

	private Dictionary<string, Player> playerPartyEntities = new Dictionary<string, Player> ();

	public void Start () {
		transform.SetParent (GameObject.FindGameObjectWithTag ("Canvas").transform, false);

		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		foreach (GameObject player in players) {
			playerPartyEntities.Add (player.GetComponent<Player> ().GetName (), player.GetComponent<Player> ());
			GameObject go = Instantiate (playerPrefab);
			go.transform.SetParent (transform);
			go.GetComponent<PlayerGameUIController> ().SetPlayer (player.GetComponent<Player> ());
		}
	}
}
