using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJS : Enemy {

	public float startAttack;

	protected override void SetStats() {
		this.startAttack = Time.time;
		this.stats = new EnemyStats (5f, 1f, 0.5f, 10);
	}

	public override void GetHit<E> (Entity<E> entity) {
		float hit = (entity.stats as PlayerStats).web;
		ChangeHealth (curHP - hit);
		base.GetHit (entity);
	}

	// ----------------------------------------------------------------------------------------------------------
	// ----------------------------------------------ANIMATIONS--------------------------------------------------
	// ----------------------------------------------------------------------------------------------------------

	protected override IEnumerator Rotate() {
		Vector2 relativePos = target.position - transform.position;
		float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg - 90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 2f);

		return GetEmptyIE ();
	}
		
	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag.Equals("Player")) {
			if (startAttack + 0.5f < Time.time) {
				startAttack = Time.time;
				Player player = coll.gameObject.GetComponent<Player> ();
				if (!player.isDead ()) {
					if (Time.time > nextAction) {
						player.GetHit (this);
						nextAction = Time.time + actionTime;
					}
				}
			}
		}
	}
}
