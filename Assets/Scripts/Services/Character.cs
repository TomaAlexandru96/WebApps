using System;
using UnityEngine;

[System.Serializable]
public class Character {
	public string name;
	public int type;

	public override string ToString () {
		return "[name: " + name + ", type: " + type.ToString () + "]";
	}

	public Sprite GetImage () {
		return AssetsConstants.GetInstance ().players[type];
	}
}

