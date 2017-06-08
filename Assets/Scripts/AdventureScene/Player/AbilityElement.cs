using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityElement : MonoBehaviour {

	public GameObject selected;
	public Text key;
	public Text abilityName;

	public Ability ability;
	private float lastUsed;

	public void Init (Ability ability, int index) {
		this.ability = ability;
		this.key.text = index.ToString ();
		this.abilityName.text = ability.ToString ();
		selected.SetActive (false);
		lastUsed = Time.time - ability.GetCooldown ();
	}

	public bool Use () {
		if (lastUsed + ability.GetCooldown () < Time.time) {
			lastUsed = Time.time;
			return true;
		}
		return false;
	}

	public void Select () {
		this.selected.SetActive (true);
	}

	public void Deselect () {
		this.selected.SetActive (false);
	}

	public void Update () {
		float deltaTime = Mathf.Clamp(Time.time - lastUsed, 0, ability.GetCooldown ());

		GetComponent<Image> ().fillAmount = deltaTime / ability.GetCooldown ();
	}
}
