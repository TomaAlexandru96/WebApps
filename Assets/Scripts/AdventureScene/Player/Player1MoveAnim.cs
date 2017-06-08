using UnityEngine;
using Photon;

public class Player1MoveAnim : Player {
	
	protected override void Animate () {
		Animator animator = GetComponent<Animator> ();

		switch (move) {
		case Direction.Still:
			animator.Play ("P1_Still");
			break;
		case Direction.Down:
			animator.Play ("P1_Down");
			break;
		case Direction.Up:
			animator.Play ("P1_Up");
			break;
		case Direction.Left:
			animator.Play ("P1_Left");
			break;
		case Direction.Right:
			animator.Play ("P1_Right");
			break;
		case Direction.UpRight:
			animator.Play ("P1_UpRight");
			break;
		case Direction.UpLeft:
			animator.Play ("P1_UpLeft");
			break;
		case Direction.DownRight:
			animator.Play ("P1_DownRight");
			break;
		case Direction.DownLeft:
			animator.Play ("P1_DownLeft");
			break;
		case Direction.Dead:
			animator.Play ("P1_Dead");
			break;
		}
	}
}