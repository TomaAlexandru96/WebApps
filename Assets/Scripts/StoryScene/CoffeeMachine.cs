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

//	InvokeRepeating ("GetHitOvertime", 1, 30);

//	public void GetHitOvertime () {
//		curHP -= 0.5f;
//	}
//
//	public void GetBuff (Buff buff) {
//		if (buff == Buff.Coffee) {
//			IncreaseHealth(10);
//		}
//	}
//
//	public void IncreaseHealth (float points) {
//		curHP = Mathf.Clamp (points + curHP, 0, stats.maxHP);
//	}
}
