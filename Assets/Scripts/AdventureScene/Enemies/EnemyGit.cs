using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGit : Enemy {

	public float startAttack;

	public override void Rotate() {
		// Do nothing
	}

	protected override IEnumerator PlayAttackAnimation() {
		GetComponent<Animator> ().Play ("EnemyGitAttackAnim");
		yield return GetEmptyIE ();
	}

	public override void PlayNormalAnimation() {
		GetComponent<Animator> ().Play ("EnemyGitAnim");
	}

	protected override void SetStats() {
		this.startAttack = Time.time;
		this.stats = new EnemyStats (7f, 2f, 0.5f);
	}

	public override void GetHit<E> (Entity<E> entity) {
		float hit = (entity.stats as PlayerStats).git;
		ChangeHealth (curHP - hit);
		base.GetHit (entity);
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag.Equals("Player")) {
			if (startAttack + 0.5f < Time.time) {
				startAttack = Time.time;
				Player player = coll.gameObject.GetComponent<Player> ();
				if (player.isDead ()) {
					PlayNormalAnimation ();
				} else {
					if (Time.time > nextAction) {
						player.GetHit (this);
						nextAction = Time.time + actionTime;
					}
				}
			}
		}
	}
}

