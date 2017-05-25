using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
	private static DontDestroyOnLoad instance = null;

	void Awake () {
		if (!IsAlreadyInit ()) {
			instance = this;
			DontDestroyOnLoad (transform.gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	private static bool IsAlreadyInit () {
		return instance != null;
	}
}
