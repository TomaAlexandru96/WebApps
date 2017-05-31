using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGit : Enemy {

	public override void Rotate() {
		// Do nothing
	}

	public override void PlayAttackAnimation() {
		animator.Play ("EnemyGitAttackAnim");
	}

	public override void PlayNormalAnimation() {
		animator.Play ("EnemyGitAnim");
	}

	public override void SetMaxHP() {
		maxHP = 7;
	}

	public override void SetDamage() {
		damage = 2;
	}
}

