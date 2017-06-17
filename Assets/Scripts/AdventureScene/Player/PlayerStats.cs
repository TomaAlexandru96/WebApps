using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats {
	
	public int level;
	public int javascript;
	public int html;
	public int python;
	public int java;
	public int mySQL;
	public int git;
	public float runSpeed = 3f;
	public float defaultStamina = 500f;
	public float runStaminaBurn = 0.05f;
	public float staminaChargeCooldown = 1f;
	public float runStaminaGain = 0.02f;

	public List<Ability> abilities = new List<Ability> () {new Ability(Ability.Mele), 
		new Ability(Ability.ForkBomb), new Ability(Ability.DebugGun), new Ability(Ability.ElectricShock)};

	public const int increaseLevelUp = 3;

	public PlayerStats (PlayerType type) {
		level = 1;
		maxHP = 30;
		speed = 1.5f;
		damage = 1f;
		switch (type) {
		case PlayerType.FrontEndDev: 
			javascript = 8;
			html = 8;
			python = 2;
			java = 2;
			mySQL = 2;
			git = 5;
			break;
		case PlayerType.BackEndDev:
			javascript = 2;
			html = 2;
			python = 8;
			java = 8;
			mySQL = 8;
			git = 5;
			break;
		case PlayerType.FullStackDev:
			javascript = 5;
			html = 5;
			python = 5;
			java = 5;
			mySQL = 5;
			git = 5;
			break;
		case PlayerType.ProductManager:
			javascript = 5;
			html = 5;
			python = 5;
			java = 5;
			mySQL = 5;
			git = 5;
			break;
		}
	}

	public void LevelUp() {
		level++;
		maxHP += increaseLevelUp;
		javascript += increaseLevelUp;
		html += increaseLevelUp;
		python += increaseLevelUp;
		java += increaseLevelUp;
		mySQL += increaseLevelUp;
		git += increaseLevelUp;
	}

}

