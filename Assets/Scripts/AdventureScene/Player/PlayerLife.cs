using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour {

	public int totHP;
	public int curHP;
	public Player1MoveAnim player;
	public float scale;
	public float currentScale;

	// Use this for initialization
	void Start () {
		player = transform.parent.gameObject.GetComponent<Player1MoveAnim> ();
		scale = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		totHP = player.stats.maxHP;
		curHP = player.curHP;
		currentScale = curHP;
		transform.localScale = new Vector3(currentScale,1f,1f);
	}
}
