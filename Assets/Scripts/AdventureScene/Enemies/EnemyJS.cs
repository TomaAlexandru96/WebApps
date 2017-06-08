﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJS : Enemy {

	protected override void SetStats() {
		this.stats = new EnemyStats (5f, 1f, 0.5f);
	}

	public override void GetHit<E> (Entity<E> entity) {
		float hit = (entity.stats as PlayerStats).javascript;
		ChangeHealth (curHP - hit);
		base.GetHit (entity);
	}

	// ----------------------------------------------------------------------------------------------------------
	// ----------------------------------------------ANIMATIONS--------------------------------------------------
	// ----------------------------------------------------------------------------------------------------------

	public override void Rotate() {
		Vector2 relativePos = target.position - transform.position;
		float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg - 90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 2f);
	}
}
