using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public Player player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetPlayer(Player player) {
		this.player = player;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player") {
			Debug.Log ("Player");
		} 

		if (coll.gameObject.tag == "Enemy") {
			Debug.Log ("Enemy");
			coll.gameObject.GetComponent<Enemy> ().GetHit (player);
			Object bulletImpact = Resources.Load ("bulletImpact");
			NetworkService.GetInstance ().SpawnScene (bulletImpact.name, transform.position, Quaternion.identity, 0);
			NetworkService.GetInstance ().Destroy (gameObject);
		}

		if (coll.gameObject.tag == "Labs") {
			NetworkService.GetInstance ().Destroy (gameObject);
		}
	}
		
}
