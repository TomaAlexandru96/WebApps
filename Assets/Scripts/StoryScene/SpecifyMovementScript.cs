using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecifyMovementScript : MonoBehaviour {

	bool guiOn = false;
	public GameObject SpecifyMovementPanel;
	private string message;

	public void Start () {
		message = "Hi there, welcome to Huxley! You can move around using W A S D ";
	}

	public void OnTriggerEnter(Collider obj){ 
		// turn message on when player is inside the trigger 
		guiOn = true; 
		Debug.Log ("lalalal");
	}

	public void onTriggerStay(Collider obj) {
		Debug.Log ("lalalal");
	}



	public void OnTriggerExit(Collider obj){
			// turn message off when player left the trigger 
		guiOn = false;
	}


	public void Update () {
		
		if (guiOn) {
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), message);
		}
	}
}
