using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerStats : EntityStats {

	public ComputerStats (float maxHP, float damage, float speed) {
		this.maxHP = maxHP;
		this.damage = damage;
		this.speed = speed;
	}

}
