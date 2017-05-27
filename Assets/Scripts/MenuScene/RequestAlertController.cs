using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RequestAlertController : MonoBehaviour {

	public Action<RequestAlertController, String> onSubmit;
	public InputField input;
	public Text question;

	public void Start () {
		input.Select ();
		input.ActivateInputField ();
	}

	public void Close () {
		Destroy (gameObject);
	}

	public void Submit () {
		onSubmit (this, input.text);
	}

	public static GameObject Create (String question, Action<RequestAlertController, String> onSubmit) {
		GameObject newAlert = Instantiate (Resources.Load<GameObject> ("Prefabs/MenuUI/RequestFriendPanel"), Vector3.zero, Quaternion.identity);

		newAlert.GetComponent<RequestAlertController> ().question.text = question;
		newAlert.GetComponent<RequestAlertController> ().onSubmit = onSubmit;
		newAlert.transform.SetParent (GameObject.FindGameObjectWithTag ("Canvas").transform, false);

		return newAlert;
	}

	public void Update () {
		if (Input.GetKeyUp (KeyCode.Return)) {
			Submit ();
		}
	}
}
