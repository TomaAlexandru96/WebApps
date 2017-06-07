using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachine : SpecifyMovementScript {

	public bool spawn;


	protected override void Conversation() {
		directionPanel.SetActive (true);
		dateTime = Time.time;
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (text[index]);
		index++;
		spawn = true;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().GetBuff (Buff.Coffee);
	}

	protected override void ExtendFunction() {
		firstContact = false;
		if (spawn && (Time.time - dateTime) > 3) {
			index = 0;
			dateTime = Time.time;
			return;
		}
	}
}
