using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameUIController : Photon.MonoBehaviour {

	public Image avatar;
	public Text playerName;
	public RectTransform healthObj;
	public static Color healthNormal = new Color32 (1, 193, 8, 255);
	public static Color healthDamaged = new Color32 (247, 239, 17, 255);
	public static Color healthDangerouslyLow = new Color32 (249, 39, 39, 255);

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
		SetHp (healthObj, player);
	}

	public static void SetHp (RectTransform healthObj, Player player) {
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
