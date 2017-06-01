using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {

	public Text username;
	public Image avatar;

	// Use this for initialization
	void Start () {
		username.text = "Name: " + CurrentUser.GetInstance ().GetUserInfo ().username;
		avatar.sprite = CurrentUser.GetInstance ().GetUserInfo ().character.GetImage ();
	}
}
