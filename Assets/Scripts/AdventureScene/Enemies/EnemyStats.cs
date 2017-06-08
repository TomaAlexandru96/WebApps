using System;

public class EnemyStats : EntityStats {
	
	public EnemyStats (float maxHP, float damage, float speed) {
		this.maxHP = maxHP;
		this.damage = damage;
		this.speed = speed;
	}
}
