using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJS : Enemy {

	public override void SetMaxHP() {
		maxHP = 5;
	}

	public override void SetDamage() {
		damage = 1;
	}
}
