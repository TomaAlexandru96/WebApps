using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3MoveAnim : Player {

	protected override void Animate () {
		Animator animator = GetComponent<Animator> ();

		switch (move) {
		case Direction.Still:
			animator.Play ("P3_Still");
			break;
		case Direction.Down:
			animator.Play ("P3_Down");
			break;
		case Direction.Up:
			animator.Play ("P3_Up");
			break;
		case Direction.Left:
			animator.Play ("P3_Left");
			break;
		case Direction.Right:
			animator.Play ("P3_Right");
			break;
		case Direction.UpRight:
			animator.Play ("P3_UpRight");
			break;
		case Direction.UpLeft:
			animator.Play ("P3_UpLeft");
			break;
		case Direction.DownRight:
			animator.Play ("P3_DownRight");
			break;
		case Direction.DownLeft:
			animator.Play ("P3_DownLeft");
			break;
		case Direction.Dead:
			animator.Play ("P3_Dead");
			break;
		}
	}
}
