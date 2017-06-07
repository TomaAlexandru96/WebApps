using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJS : Enemy {

	public override void SetStats() {
		this.stats = new EnemyStats (5f, 1f, 0.5f);
	}

	public override void GetHit (Player player) {
		float hit = player.stats.javascript;

		if (curHP > hit) {
			curHP -= hit;
		} else {
			curHP = 0;
			gameObject.SetActive (false);
		}
	}

	public virtual void Rotate() {
		Vector2 relativePos = target.position - transform.position;
		float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg - 90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 2f);
	}
}
