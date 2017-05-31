using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {
	
	private List<Item> items = new List<Item>();

	public void SetBasic(List<Item> basic)
	{
		items = basic;
	}

	public void AddItem(Item item) {

		Item newItem = new Item(item.name, item.damage, item.defense); 

		items.Add(newItem);
	}
}
