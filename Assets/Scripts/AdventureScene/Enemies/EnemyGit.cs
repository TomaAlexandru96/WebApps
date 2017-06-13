using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGit : Enemy {

	public float startAttack;

	protected override void SetStats() {
		this.startAttack = Time.time;
		this.stats = new EnemyStats (7f, 2f, 0.5f);
	}

	public override void GetHit<E> (Entity<E> entity) {
		float hit = 0;
		if (entity.stats.GetType() == typeof(PlayerStats)) {
			hit = (entity.stats as PlayerStats).git;

		} else if (entity.stats.GetType() == typeof(ComputerStats)) {
			hit = entity.stats.damage;
		}

		ChangeHealth (curHP - hit);
		base.GetHit (entity);

	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag.Equals("Player")) {
			if (startAttack + 0.5f < Time.time) {
				startAttack = Time.time;
				Player player = coll.gameObject.GetComponent<Player> ();
				if (player.isDead ()) {
					PlayAnimation ("PlayNormalAnimation");
				} else {
					if (Time.time > nextAction) {
						player.GetHit (this);
						nextAction = Time.time + actionTime;
					}
				}
			}
		}
	}

	// ----------------------------------------------------------------------------------------------------------
	// ----------------------------------------------ANIMATIONS--------------------------------------------------
	// ----------------------------------------------------------------------------------------------------------

	protected override IEnumerator Rotate() {
		return GetEmptyIE ();
	}

	protected override IEnumerator PlayAttackAnimation() {
		GetComponent<Animator> ().Play ("EnemyGitAttackAnim");
		yield return GetEmptyIE ();
	}

	protected override IEnumerator PlayNormalAnimation() {
		GetComponent<Animator> ().Play ("EnemyGitAnim");
		yield return GetEmptyIE ();
	}
}

