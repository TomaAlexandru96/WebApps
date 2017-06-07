using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour {

	public Transform target;
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
	public bool onSpecialCaseMovement;
	public DateTime spectialCaseMovEnd;
	public DateTime lastCollisionTime;
	public Vector3 specialCaseDirection;


	// Use this for initialization
	void Start () {
		transform.SetParent (GameObject.FindGameObjectWithTag ("Grid").transform);
		lastCollisionTime = DateTime.Now;
		spectialCaseMovEnd = DateTime.MinValue;
		onSpecialCaseMovement = false;
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		grid = GetComponentInParent<Grid> ();
		speed = 0.5f;
		targetInRange = false;
		SetDamage ();
		SetMaxHP ();
		curHP = maxHP;
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

	private bool FindNewTarget () {
		var targets = GameObject.FindGameObjectsWithTag ("Player");
		if (targets.Length == 0) {
			return false;
		}
		target = targets [UnityEngine.Random.Range (0, targets.Length - 1)].transform;
		return true;
	}

	private bool HasTarget () {
		return target != null;
	}

	// Update is called once per frame
	void Update () {
		if (!HasTarget ()) {
			if (!FindNewTarget ()) {
				return;
			}
		}

		if (targetInRange) {
			/*RaycastHit2D hit = Physics2D.Raycast (transform.position, 
				target.position - transform.position, Vector2.Distance(transform.position, target.position), 
				LayerMask.GetMask (new string[]{ "Map" }));
			if (hit == null) {
				Debug.Log ("Normal Movement");
				// Normal movement
				Rotate ();
				//only move if I'm far away from the target
				if (Vector2.Distance (transform.position, target.position) > 0.5f
				    && !attackingPlayer) {
					MoveEnemy ();
				}
			} else {*/
			onSpecialCaseMovement = (DateTime.Now - spectialCaseMovEnd).Seconds < 1;
				// pathfinding
			Point curTargetPos = CurrentTargetPoint();
				// RECALCULATE
			if (!curTargetPos.Equals (lastTargetPos)) {
				lastTargetPos = curTargetPos;

				if (curTargetPos != null) {
					Point enemyPos = CurrentEnemyPoint();

					currentBr = PathFinder.FindPath (grid, enemyPos, curTargetPos);
					// grid.DrawPath (currentBr);
					currentBr = currentBr.next;
				}
			}
			// MOVEMENT
			MoveEnemy ();
		
		}
	}

	public Point CurrentTargetPoint() {
		return new Point (Mathf.RoundToInt((target.localPosition.x * 2)), Mathf.RoundToInt(target.localPosition.y * 2));
	}

	public Point CurrentEnemyPoint() {
		return new Point (Mathf.RoundToInt((transform.localPosition.x * 2)), Mathf.RoundToInt(transform.localPosition.y * 2));
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			PlayAttackAnimation ();
			attackingPlayer = true;
		}
		lastCollisionTime = DateTime.Now;
	}

	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			PlayNormalAnimation ();
			attackingPlayer = false;
		}
		lastCollisionTime = DateTime.Now;

	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			Player player = coll.gameObject.GetComponent<Player> ();
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
		} else {
			onSpecialCaseMovement = (DateTime.Now - lastCollisionTime).Seconds > 1;
			if (onSpecialCaseMovement) {
				spectialCaseMovEnd = DateTime.Now;
				specialCaseDirection = new Vector3 (coll.contacts [0].point.x, coll.contacts [0].point.y, 0) - transform.position;
			}

			//Random.Range random = new Random ();
			float randomInt = UnityEngine.Random.Range(0.0f,5.0f);

			if (randomInt < 1) {
				specialCaseDirection = new Vector3 (-specialCaseDirection.x, -specialCaseDirection.y, 0);
			} else if (randomInt < 2) {
				specialCaseDirection = new Vector3 (-specialCaseDirection.y, specialCaseDirection.x, 0);
			} else if (randomInt < 3) {
				specialCaseDirection = new Vector3 (+specialCaseDirection.y, -specialCaseDirection.x, 0);
			} else {
				specialCaseDirection = target.position - transform.position;
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
		if (onSpecialCaseMovement ) {
			Vector3 movement = specialCaseDirection.normalized * speed;
			rb.velocity = movement;
		} else {
			if (currentBr != null) {
				Vector2 bcRealPos = currentBr.toRealCoordinates (grid);
				if (Vector2.Distance (transform.position, 
					new Vector2 (bcRealPos.x, bcRealPos.y)) > 0.1f) {
					Vector3 movement = new Vector2 (bcRealPos.x - transform.position.x,
						bcRealPos.y - transform.position.y);
					rb.velocity = movement.normalized * 0.8f;
				} else {
					currentBr = currentBr.next;
				}
			} else {
				rb.velocity = new Vector2(0,0);
			}
								
		}
	}

	public virtual void GetHit(int hit) {
		if (curHP > hit) {
			curHP -= hit;
		} else {
			curHP = 0;
			gameObject.SetActive (false);
		}
	}
}
