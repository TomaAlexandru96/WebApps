using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerStats : EntityStats {

	public int web = 8;
	public int functional = 8;
	public int oo = 8;
	public int git = 8;

	public ComputerStats (float maxHP, float damage, float speed) {
		this.maxHP = maxHP;
		this.damage = damage;
		this.speed = speed;
	}
}
