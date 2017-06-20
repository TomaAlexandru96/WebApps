using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {

	public User user = CurrentUser.GetInstance ().GetUserInfo ();
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
		if (avatar != null) {
			avatar.sprite = user.character.GetImage ();	
		}

		PlayerStats stats = user.character.GetStats ();
		characterName.text = user.character.name;
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

	public void SetUser (User user) {
		this.user = user;
		UpdateInfo ();
	}
}
