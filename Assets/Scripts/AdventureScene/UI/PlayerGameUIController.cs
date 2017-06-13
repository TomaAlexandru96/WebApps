using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerGameUIController : NetworkBehaviour {

	public Image avatar;
	public Text playerName;
	public RectTransform healthObj;
	public Color healthNormal;
	public Color healthDamaged;
	public Color healthDangerouslyLow;

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
			healthObj.GetComponent<Image> ().color = healthDamaged;
		} else if (newHP.x < 0.2) {
			healthObj.GetComponent<Image> ().color = healthDangerouslyLow;
		} else {
			healthObj.GetComponent<Image> ().color = healthNormal;
		}
	}
}
