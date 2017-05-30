using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int playerDamage;
	public Transform target;
	public float speed;
	public int damage;

	// Use this for initialization
	void Start () {
		speed = 0.5f;
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		damage = 1;
	}
	
	// Update is called once per frame
	void Update () {
		MoveEnemy ();
	}

	public void MoveEnemy () {
		transform.LookAt(target);
		transform.Rotate(new Vector3(0,-90,-90), Space.Self);

		//only move if I'm far away from the target
		if (Vector2.Distance(transform.position,target.position)>1f) {
			transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
		}
	}
}
