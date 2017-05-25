using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdventureController : MonoBehaviour {

	public void ExitGame () {
		SceneManager.LoadScene ("Menu");
	}
}
