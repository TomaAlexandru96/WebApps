using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyEntry : MonoBehaviour {

	public Text usernameText;

	public void ChangeName (string username) {
		usernameText.text = username;
	}

	public void Start () {
		DBServer.GetInstance ().FindUser (usernameText.text, (user) => {
			transform.GetChild (0).GetComponentInChildren <Image> ().sprite = user.character.GetImage ();
		}, (error) => {
			Debug.LogError (error);
		});
	}
}
