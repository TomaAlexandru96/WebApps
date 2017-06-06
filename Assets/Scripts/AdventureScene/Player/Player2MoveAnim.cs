using UnityEngine;

public class Player2MoveAnim : Player {
	
	protected override void Animate () {
		switch (move) {
		case Direction.Still:
			animator.Play ("P2_Still");
			break;
		case Direction.Down:
			animator.Play ("P2_Down");
			break;
		case Direction.Up:
			animator.Play ("P2_Up");
			break;
		case Direction.Left:
			animator.Play ("P2_Left");
			break;
		case Direction.Right:
			animator.Play ("P2_Right");
			break;
		case Direction.UpRight:
			animator.Play ("P2_UpRight");
			break;
		case Direction.UpLeft:
			animator.Play ("P2_UpLeft");
			break;
		case Direction.DownRight:
			animator.Play ("P2_DownRight");
			break;
		case Direction.DownLeft:
			animator.Play ("P2_DownLeft");
			break;

		}
	}
}