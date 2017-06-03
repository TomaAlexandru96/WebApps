using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chooseAvatarType : MonoBehaviour {

	public int characterNumber;
	public GameObject chooseAvatar;

	public void AvatarSelected() {
		chooseAvatar.GetComponent<AvatarTypePanel> ().characterNumber = characterNumber;
		chooseAvatar.GetComponent<AvatarTypePanel> ().AvatarChosen (characterNumber);
	}
}
