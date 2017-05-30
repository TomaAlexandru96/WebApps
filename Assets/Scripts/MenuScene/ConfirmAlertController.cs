using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmAlertController : MonoBehaviour {

	public Action<ConfirmAlertController> onYes;
	public Action<ConfirmAlertController> onNo;
	public Text question;

	public void Update () {
	}

	public void Close () {
		Destroy (gameObject);
	}

	public static GameObject Create (String question, Action<ConfirmAlertController> onYes, Action<ConfirmAlertController> onNo) {
		GameObject newAlert = Instantiate (Resources.Load<GameObject> ("Prefabs/MenuUI/ConfirmPanel"), Vector3.zero, Quaternion.identity);

		newAlert.GetComponent<ConfirmAlertController> ().question.text = question;
		newAlert.GetComponent<ConfirmAlertController> ().onYes = onYes;
		newAlert.GetComponent<ConfirmAlertController> ().onNo = onNo;
		newAlert.transform.SetParent (GameObject.FindGameObjectWithTag ("Canvas").transform, false);

		return newAlert;
	}

	public void OnYes () {
		onYes (this);
	}

	public void OnNo () {
		onNo (this);
	}
}
