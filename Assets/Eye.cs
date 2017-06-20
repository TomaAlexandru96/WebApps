using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour {

	Transform player;
	Vector3 startPos;

	// Use this for initialization
	void Start () {
		startPos = transform.localPosition;
		player = GetComponentInParent<EnemyProlog> ().target;
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			Debug.Log ("No target");
			player = GetComponentInParent<EnemyProlog> ().target;
			transform.localPosition = startPos;
		} else {
			Debug.Log ("Target");
			Vector2 relativePos = player.position - transform.position;
			Debug.Log (relativePos);
			float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg - 40;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 2f);
			player = GetComponentInParent<EnemyProlog> ().target;
		}
	}
}
