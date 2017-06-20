using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProlog : Enemy {

	protected override void SetStats() {
		this.stats = new EnemyStats (5f, 1f, 0.5f, 10);
	}

	public override void GetHit<E> (Entity<E> entity) {
		float hit = (entity.stats as PlayerStats).functional;
		ChangeHealth (curHP - hit);
		base.GetHit (entity);
	}

	// ----------------------------------------------------------------------------------------------------------
	// ----------------------------------------------ANIMATIONS--------------------------------------------------
	// ----------------------------------------------------------------------------------------------------------

	protected override IEnumerator Rotate() {
		return GetEmptyIE ();
	}
}