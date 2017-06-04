using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public Transform target;
	public GameObject enemy;
	public float speed;
	public int damage;
	public int maxHP;
	public int curHP;
	private Rigidbody2D rb;
	public bool attackingPlayer;
	public float actionTime = 1f;
	public float nextAction = 0;
	public Animator animator;
	public Grid grid;
	public bool targetInRange;
	public Point lastTargetPos;
	public BreadCrumb currentBr;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		grid = GetComponentInParent<Grid> ();
		speed = 0.5f;
		targetInRange = false;
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		SetDamage ();
		SetMaxHP ();
		curHP = maxHP;
		lastTargetPos = new Point((int)(target.localPosition.x*2), (int)(target.localPosition.y*2));
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player") {
			targetInRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player") {
			targetInRange = false;
		}
	}

	// Update is called once per frame
	void Update () {
		// pathfinding
		if (targetInRange) {
			Point cur = new Point((int)(target.localPosition.x*2), (int)(target.localPosition.y*2));
			if (cur != lastTargetPos) {
				lastTargetPos = cur;

				Point targetPos = new Point ((int)(target.localPosition.x * 2), (int)(target.localPosition.y * 2));

				if (targetPos != null) {
					Point enemyPos = new Point ((int)(transform.localPosition.x * 2), (int)(transform.localPosition.y * 2));

					BreadCrumb currentBr = PathFinder.FindPath (grid, enemyPos, targetPos);
				}
			} else {
				if (currentBr != null) {
					if (transform.position.x - (currentBr.position.X * 0.5f) + transform.position.x > 1
					    && transform.position.y - (currentBr.position.Y * 0.5f) + transform.position.y > 1) {
						Vector3 movement = 
							(new Vector3 (target.position.x - transform.position.y, target.position.y - transform.position.y, 0)).normalized * speed;
						rb.velocity = movement;
					} else {
						currentBr = currentBr.next;
					}
				}
			}
		}


		// Old movement
		//Rotate ();
		//only move if I'm far away from the target
		/*if (Vector2.Distance (transform.position, target.position) > 0.5f
			&& !attackingPlayer) {
			MoveEnemy ();
		}*/
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

	public virtual void GetHit(int hit) {
		if (curHP > hit) {
			curHP -= hit;
		} else {
			curHP = 0;
			enemy.SetActive (false);
		}
	}
}
