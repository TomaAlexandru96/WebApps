using UnityEngine;

public class P2_MoveAnim : MonoBehaviour {

	Animator animator;

	public float speed;

	private Rigidbody2D rb; 

	public Direction move;

	public enum Direction
	{
		Still, Down, Up, Left, Right, UpRight, UpLeft, DownRight, DownLeft  
	}

	/*int stillHash = Animator.StringToHash("Still");
	int upHash = Animator.StringToHash("Up");
	int downHash = Animator.StringToHash("Down");
	int leftHash = Animator.StringToHash("Left");
	int rightHash = Animator.StringToHash("Right");
	int upRightHash = Animator.StringToHash("UpRight");
	int downRightHash = Animator.StringToHash("DownRight");
	int upLeftHash = Animator.StringToHash("UpLeft");
	int downLeftHash = Animator.StringToHash("DownLeft");*/

	void Start() {
		rb = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator>();
	}

	void Update() {

		float h = Input.GetAxisRaw ("Horizontal");

		float v = Input.GetAxisRaw ("Vertical");


		// RIGHT
		if (h > 0.1) {

			if (v > 0.1) {
				move = Direction.UpRight;
			} else if (v < -0.1) {
				move = Direction.DownRight;
			} else {
				move = Direction.Right;
			}
			// LEFT
		} else if (h < -0.1) {

			if (v > 0.1) {
				move = Direction.UpLeft;
			} else if (v < -0.1) {
				move = Direction.DownLeft;
			} else {
				move = Direction.Left;
			}
			// STILL
		} else {

			if (v > 0.1) {
				move = Direction.Up;
			} else if (v < -0.1) {
				move = Direction.Down;
			} else {
				move = Direction.Still;
			}

		}

		determineAnimation ();

		Vector2 movement = new Vector2 (h, v).normalized;

		rb.velocity = movement * speed;

	}

	void determineAnimation() {

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