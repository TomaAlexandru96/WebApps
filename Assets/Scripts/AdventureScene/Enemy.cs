using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public Transform target;
	public float speed;
	public int damage;
	public int maxHP;
	private Rigidbody2D rb;
	public bool attackingPlayer;
	public float actionTime = 1f;
	public float nextAction = 0;
	public Animator animator;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		speed = 0.5f;
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		SetDamage ();
		SetMaxHP ();
	}

	// Update is called once per frame
	void Update () {
		Rotate ();
		//only move if I'm far away from the target
		if (Vector2.Distance (transform.position, target.position) > 0.5f
			&& !attackingPlayer) {
			MoveEnemy ();
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			PlayAttackAnimation ();
			attackingPlayer = true;
		}
	}

	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			PlayNormalAnimation ();
			attackingPlayer = false;
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			P1_MoveAnim player = coll.gameObject.GetComponent<P1_MoveAnim> ();
			if (player.dead) {
				PlayNormalAnimation ();
			} else {
				if (Time.time > nextAction) {
					player.Damaged ();
					nextAction = Time.time + actionTime;
					player.curHP -= damage;
					if (player.curHP <= 0) {
						player.curHP = 0;
						player.dead = true;
					}
				}
			}
		}
	}

	public virtual void SetMaxHP() {
		// use child function
	}

	public virtual void SetDamage() {
		// use child function
	}

	public virtual void PlayAttackAnimation() {
		// use child function
	}

	public virtual void PlayNormalAnimation() {
		// use child function
	}

	public virtual void Rotate() {
		Vector2 relativePos = target.position - transform.position;
		float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg - 90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 2f);
	}

	public virtual void MoveEnemy () {
		Vector3 movement = (target.position - transform.position).normalized * speed;
		rb.velocity = movement;
	}
}
