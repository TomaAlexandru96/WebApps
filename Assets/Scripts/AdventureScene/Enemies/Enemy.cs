using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour {

	public Transform target;
	public float curHP;
	public EnemyStats stats;
	public bool attackingPlayer;
	public float actionTime = 1f;
	public float nextAction = 0;
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
		grid = GetComponentInParent<Grid> ();
		targetInRange = false;
		SetStats ();
		curHP = stats.maxHP;
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

	public virtual void SetStats () {
		// use child function
	}

	public virtual void PlayAttackAnimation () {
		// use child function
	}

	public virtual void PlayNormalAnimation () {
		// use child function
	}

	public virtual void GetHit (Player player) {
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
			Vector3 movement = specialCaseDirection.normalized * stats.speed;
			GetComponent<Rigidbody2D> ().velocity = movement;
		} else {
			if (currentBr != null) {
				Vector2 bcRealPos = currentBr.toRealCoordinates (grid);
				if (Vector2.Distance (transform.position, 
					new Vector2 (bcRealPos.x, bcRealPos.y)) > 0.1f) {
					Vector3 movement = new Vector2 (bcRealPos.x - transform.position.x,
						bcRealPos.y - transform.position.y);
					GetComponent<Rigidbody2D> ().velocity = movement.normalized * 0.8f;
				} else {
					currentBr = currentBr.next;
				}
			} else {
				GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
			}
		}
	}
}
