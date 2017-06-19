using System;

public class EnemyStats : EntityStats {

	public int xpReward;

	public EnemyStats (float maxHP, float damage, float speed, int xpReward) {
		this.maxHP = maxHP;
		this.damage = damage;
		this.speed = speed;
		this.xpReward = xpReward;
	}
}
