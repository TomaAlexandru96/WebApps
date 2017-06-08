using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour {

	public GameObject abilityPrefab;
	public RectTransform sprintBar;
	private Player player;
	private List<AbilityElement> abilities = new List<AbilityElement> ();
	private AbilityElement selected;
	public float stamina;
	public float lastSprint;

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

		stamina = player.stats.defaultStamina;
		lastSprint = Time.time;
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

	public bool Sprint () {
		stamina = Mathf.Clamp (stamina - player.stats.runStaminaBurn, 0, player.stats.defaultStamina);
		lastSprint = Time.time;
		return stamina != 0;
	}

	public void Update () {
		sprintBar.localScale = Vector3.Lerp (sprintBar.localScale, 
						new Vector3 (stamina / player.stats.defaultStamina, 1, 1), 0.1f);

		if (lastSprint + player.stats.staminaChargeCooldown < Time.time) {
			stamina = Mathf.Clamp (stamina + player.stats.runStaminaGain, 0, player.stats.defaultStamina);
		}
	}
}
