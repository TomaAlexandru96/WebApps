using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats {
	
	public int xp;
	public int web;
	public int functional;
	public int oo;
	public int git;
	public PlayerType pType;
	public float runSpeed = 3f;
	public float defaultStamina = 500f;
	public float runStaminaBurn = 0.05f;
	public float staminaChargeCooldown = 1f;
	public float runStaminaGain = 0.02f;

	private List<Ability> abilities = new List<Ability> () {new Ability (Ability.Mele), 
		new Ability (Ability.ForkBomb), new Ability (Ability.DebugGun), new Ability (Ability.ElectricShock)
	};

	public PlayerStats (PlayerType type, int xp) {
		this.xp = xp;
		this.pType = type;
		maxHP = 30;
		speed = 1.5f;
		damage = 1f;

		xp = 1;
		web = 1;
		functional = 1;
		oo = 1;

		switch (type) {
		case PlayerType.FrontEndDev: 
			web = 8;
			git = 5;
			break;
		case PlayerType.BackEndDev:
			web = 2;
			git = 5;
			break;
		case PlayerType.FullStackDev:
			web = 5;
			break;
		case PlayerType.ProductManager:
			web = 5;
			git = 5;
			break;
		}
	}

	public List<Ability> GetAbilities () {
		return abilities;
	}

	public int GetLevel () {
		return 9000;
	}

	public int GetNextMilestoneXP () {
		return 350;
	}
}

