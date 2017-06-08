using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player4MoveAnim : Player {

	protected override void Animate () {
		Animator animator = GetComponent<Animator> ();

		switch (move) {
		case Direction.Still:
			animator.Play ("P4_Still");
			break;
		case Direction.Down:
			animator.Play ("P4_Down");
			break;
		case Direction.Up:
			animator.Play ("P4_Up");
			break;
		case Direction.Left:
			animator.Play ("P4_Left");
			break;
		case Direction.Right:
			animator.Play ("P4_Right");
			break;
		case Direction.UpRight:
			animator.Play ("P4_UpRight");
			break;
		case Direction.UpLeft:
			animator.Play ("P4_UpLeft");
			break;
		case Direction.DownRight:
			animator.Play ("P4_DownRight");
			break;
		case Direction.DownLeft:
			animator.Play ("P4_DownLeft");
			break;
		case Direction.Dead:
			animator.Play ("P4_Dead");
			break;
		}
	}
}
