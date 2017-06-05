using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameUIController : Photon.MonoBehaviour {

	public Image avatar;
	public Text playerName;

	public void SetStats (string name) {
		DBServer.GetInstance ().FindUser (name, (user) => {
			playerName.text = user.username;
			avatar.sprite = user.character.GetImage ();
		}, (error) => {
			Debug.LogError (error);	
		});
	}
}
