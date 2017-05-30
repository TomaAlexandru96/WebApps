using System;

using UnityEngine;

public class Player {

	public PlayerStats stats;
	public int currentHP;
	public int exp;

	public Player() {

		stats = new PlayerStats (PlayerType.FrontEndDev);
		currentHP = stats.maxHP;
		exp = 0;
	}

}

