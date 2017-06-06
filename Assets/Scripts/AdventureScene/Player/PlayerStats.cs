using System;

public class PlayerStats {

	public int level;
	public int maxHP;
	public int javascript;
	public int html;
	public int python;
	public int java;
	public int mySQL;
	public int git;

	public const int increaseLevelUp = 3;

	public PlayerStats(PlayerType type) {
		level = 1;
		maxHP = 20;
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

