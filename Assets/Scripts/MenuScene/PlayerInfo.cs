using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {

	public Image avatar;
	public Text characterName;
	public Text webStats;
	public Text funcStats;
	public Text ooStats;
	public Text gitStats;
	public Text pType;
	public Text level;
	public RectTransform xpFill;

	void Start () {
		InvokeRepeating ("UpdateInfo", 0, 100f);
	}

	// Use this for initialization
	void UpdateInfo () {
		avatar.sprite = CurrentUser.GetInstance ().GetUserInfo ().character.GetImage ();

		PlayerStats stats = CurrentUser.GetInstance ().GetUserInfo ().character.GetStats ();
		characterName.text = CurrentUser.GetInstance ().GetUserInfo ().character.name;
		webStats.text = stats.web.ToString ();
		funcStats.text = stats.functional.ToString ();
		ooStats.text = stats.oo.ToString ();
		gitStats.text = stats.git.ToString ();
		pType.text = stats.pType.ToString ();
		level.text = stats.GetLevel ().ToString ();

		Vector3 xpFillScale = xpFill.localScale;
		xpFillScale.x = (float) (stats.xp % stats.baseLevelXP) / stats.baseLevelXP;
		xpFill.localScale = xpFillScale;
	}
}
