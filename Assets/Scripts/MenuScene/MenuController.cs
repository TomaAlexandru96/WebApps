using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public void Logout () {
		DBServer.Logout ();
		CurrentUser.GetInstance ().Logout ();
		SceneManager.LoadScene ("Login");
	}
}
