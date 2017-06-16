using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingScript : MonoBehaviour {

	private float pingTest;

	public void Start () {
		pingTest = Time.time;
	}

	public void Update() {
		if (pingTest + 2f < Time.time) {
			pingTest = Time.time;
			GetComponent<Text> ().text = PhotonNetwork.GetPing().ToString ();
		}
	}
}
