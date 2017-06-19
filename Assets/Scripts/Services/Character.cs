using System;
using UnityEngine;

[System.Serializable]
public class Character {
	public string name;
	public int type;
	public int xp;
	public PlayerType pType;

	public override string ToString () {
		return "[name: " + name + ", type: " + type.ToString () + "]";
	}

	public Sprite GetImage () {
		return AssetsConstants.GetInstance ().players[type];
	}

	public PlayerStats GetStats () {
		return new PlayerStats (pType, xp);
	}
}