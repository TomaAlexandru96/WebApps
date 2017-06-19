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

	private int levelIncrease = 3;
	public int baseLevelXP = 350;

	private List<Ability> abilities = new List<Ability> () {new Ability (Ability.Mele), 
		new Ability (Ability.ForkBomb), new Ability (Ability.DebugGun), new Ability (Ability.ElectricShock)
	};

	public PlayerStats (PlayerType type, int xp) {
		this.pType = type;
		maxHP = 30;
		speed = 1.5f;
		damage = 1f;

		GainXp (xp);
	}

	public void GainXp (int xp) {
		this.xp += xp;

		web = GetLevel () * levelIncrease;
		functional = GetLevel () * levelIncrease;
		oo = GetLevel () * levelIncrease;
		git = GetLevel () * levelIncrease;

		switch (pType) {
		case PlayerType.FrontEndDev: 
			web += 8;
			git += 5;
			break;
		case PlayerType.BackEndDev:
			web += 2;
			git += 5;
			break;
		case PlayerType.FullStackDev:
			web += 5;
			break;
		case PlayerType.ProductManager:
			web += 5;
			git += 5;
			break;
		}
	}

	public List<Ability> GetAbilities () {
		return abilities;
	}

	public int GetLevel () {
		return xp / baseLevelXP + 1;
	}

	public int GetNextMilestoneXP () {
		if (GetLevel () * baseLevelXP < xp) {
			return (GetLevel () + 1) * baseLevelXP;
		} else {
			return GetLevel () * baseLevelXP;
		}
	}
}

