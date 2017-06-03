using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AvatarTypePanel : MonoBehaviour {

	public List<GameObject> avatars = new List<GameObject> ();
	public int characterNumber;

	public void RequestCharacter () {
		SceneManager.LoadScene ("Menu");
	}

		public void AvatarChosen(int num) {
		foreach (GameObject avatar in avatars) {
			avatar.transform.GetComponent<Image> ().color = new Color32 (200, 200, 200, 100);
		}
		avatars [num].transform.GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);
	}
}
