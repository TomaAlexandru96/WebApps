using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetsConstants : MonoBehaviour {

	public Sprite[] players;

	private static AssetsConstants instance = null;

	void Awake () {
		if (instance == null) {
			AssetsConstants.instance = this;
		}
	}

	public static AssetsConstants GetInstance () {
		return AssetsConstants.instance;
	}
}
