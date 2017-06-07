﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameUIController : Photon.PunBehaviour {

	public Image avatar;
	public Text playerName;
	public RectTransform healthObj;

	private Player player;

	public void SetPlayer (Player player) {
		this.player = player;
		DBServer.GetInstance ().FindUser (player.GetName (), (user) => {
			playerName.text = user.username;
			avatar.sprite = user.character.GetImage ();
		}, (error) => {
			Debug.LogError (error);	
		});
	}

	public void Update () {
		if (player == null) {
			return;
		}
		Vector2 newHP = Vector2.Lerp (healthObj.localScale, new Vector2 ((float) player.curHP / (float) player.stats.maxHP, 1), 0.1f);
		healthObj.localScale = newHP;
		if (newHP.x >= 0.2 && newHP.x < 0.5) {
			healthObj.GetComponent<Image> ().color = Color.yellow;
		} else if (newHP.x < 0.2) {
			healthObj.GetComponent<Image> ().color = Color.red;
		} else {
			healthObj.GetComponent<Image> ().color = Color.green;
		}
	}
}
