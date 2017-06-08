using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGit : Enemy {

	public float startAttack;

	public override void Rotate() {
		// Do nothing
	}

	public override void PlayAttackAnimation() {
		GetComponent<Animator> ().Play ("EnemyGitAttackAnim");
	}

	public override void PlayNormalAnimation() {
		GetComponent<Animator> ().Play ("EnemyGitAnim");
	}

	public override void SetStats() {
		this.startAttack = Time.time;
		this.stats = new EnemyStats (7f, 2f, 0.5f);
	}

	public override void GetHit (Player player) {
		float hit = player.stats.git;

		if (curHP > hit) {
			curHP -= hit;
		} else {
			curHP = 0;
			gameObject.SetActive (false);
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag.Equals("Player")) {
			if (startAttack + 0.5f < Time.time) {
				startAttack = Time.time;
				Player player = coll.gameObject.GetComponent<Player> ();
				if (player.dead) {
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

