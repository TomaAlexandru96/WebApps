using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller308 : MonoBehaviour {

	public Sprite withoutFood;
	public Sprite withFood;
	private Transform huxely308;

	void Start () {
		huxely308 = GameObject.FindGameObjectWithTag ("Huxely308").transform;
		huxely308.GetComponent<SpriteRenderer> ().sprite = withoutFood;
	}

	public void SetFood () {
		huxely308.GetComponent<SpriteRenderer> ().sprite = withFood;
	}

	public void DissapearFood () {
		huxely308.GetComponent<SpriteRenderer> ().sprite = withoutFood;
	}

	public void BlankScreen () {
		huxely308.GetComponent<SpriteRenderer> ().sprite = null;
	}
}
