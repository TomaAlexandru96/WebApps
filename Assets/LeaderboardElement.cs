using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardElement : MonoBehaviour {

	public Text nr;
	public Text username;
	public Text experience;

	public void Init (int nr, User user) {
		this.nr.text = nr.ToString ();
		username.text = user.username;
		experience.text = user.character.xp.ToString ();
	}
}
