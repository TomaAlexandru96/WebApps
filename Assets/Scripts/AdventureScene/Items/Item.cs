using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

	public int damage;
	public int defense;
	public string name;
	public bool longRange;

	public Item(string name, int damage, int defense, bool longRange) {
		this.name = name;
		this.damage = damage;
		this.defense = defense;
		this.longRange = longRange;
	}

}
