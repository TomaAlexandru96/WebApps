using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoController : MonoBehaviour {

	public Image avatar;
	public Text characterName;
	public Text webStats;
	public Text funcStats;
	public Text ooStats;
	public Text gitStats;
	public GameObject xpBar;
	public Text pType;
	public RectTransform xpFill;
	public Text level;
	public Color healthNormal;
	public Color healthDamaged;
	public Color healthDangerouslyLow;
	public RectTransform healthObj;
	public Player player;

	public void Init (Player player) {
		this.player = player;
		UpdateInfo (player.user);
	}

	public void UpdateInfo (User info) {
		PlayerStats stats = info.character.GetStats ();
		avatar.sprite = info.character.GetImage ();
		characterName.text = info.character.name;
		webStats.text = stats.web.ToString ();
		funcStats.text = stats.functional.ToString ();
		ooStats.text = stats.oo.ToString ();
		gitStats.text = stats.git.ToString ();
		pType.text = stats.pType.ToString ();
		level.text = stats.GetLevel ().ToString ();

		Vector3 xpFillScale = xpFill.localScale;
		xpFillScale.x = (float) stats.xp / stats.GetNextMilestoneXP ();
		xpFill.localScale = xpFillScale;

		PlayerGameUIController.SetHp (healthObj, player);
	}

	void Update () {
		if (player == null) {
			return;
		}

		UpdateInfo (player.user);
	}
}
