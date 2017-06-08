using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour {

	public GameObject abilityPrefab;
	private Player player;
	private List<AbilityElement> abilities = new List<AbilityElement> ();
	private AbilityElement selected;

	public void Init (Player player) {
		this.player = player;
		int i = 1;
		foreach (var ability in this.player.stats.abilities) {
			GameObject newAbility = Instantiate (abilityPrefab);
			newAbility.transform.SetParent (transform);
			newAbility.GetComponent<AbilityElement> ().Init (ability, i);
			abilities.Add (newAbility.GetComponent<AbilityElement> ());
			i++;
		}
		selected = abilities [0];
		selected.Select ();
	}

	public bool UseAbility () {
		return selected.Use ();
	}

	public void SelectAbility (int index) {
		selected.Deselect ();
		selected = abilities [index - 1];
		selected.Select ();
	}

	public Ability GetSelectedAbility () {
		return selected.ability;
	}
}
