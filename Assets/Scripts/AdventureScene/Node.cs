using UnityEngine;
using System.Collections;

public class Node {

	public bool BadNode;    

	//Grid coordinates
	public int X;
	public int Y;

	//world position
	public Vector2 Position;

	//our 8 connection points
	public NodeConnection Up;
	public NodeConnection Left;
	public NodeConnection Down;
	public NodeConnection Right;
	public NodeConnection UpLeft;
	public NodeConnection UpRight;
	public NodeConnection DownLeft;
	public NodeConnection DownRight;

	public GameObject Draw;
	public Node node;

	public Node(float x, float y, Vector2 position, Grid grid) {
		Initialize(x, y, position, grid);
	}

	public void Initialize(float x, float y, Vector2 position, Grid grid) {
		X = (int)x;
		Y = (int)y;
		//Debug.Log ("X: " + x + "Y: " + y);
		Position = position;

		RaycastHit2D hit = Physics2D.Raycast(new Vector2(Position.x, Position.y), new Vector3(0,0,-1), 1, LayerMask.GetMask (new string[]{"Map"}));
		if (hit.collider != null) {
			DisableConnections ();
			BadNode = true;
		}

		//Draw Node on screen for debugging purposes
		/*Draw = GameObject.Instantiate (Resources.Load ("Node")) as GameObject;
		Draw.transform.position = Position;
		if (BadNode) {
			Draw.GetComponent<SpriteRenderer> ().color = Color.red;
		} else {
			Draw.GetComponent<SpriteRenderer> ().color = Color.blue;
		}*/
	}

	/*public void SetColor(Color color) {
		Draw.transform.GetComponent<SpriteRenderer> ().color = color;
	}*/

	//Cull nodes if they don't have enough valid connection points (3)
	public void CheckConnectionsPass1(Grid grid) {
		if (!BadNode) {

			int clearCount = 0;

			if (Up == null || !Up.Valid)
				clearCount++;
			if (Down == null || !Down.Valid)
				clearCount++;
			if (Left == null || !Left.Valid)
				clearCount++;
			if (Right == null || !Right.Valid)
				clearCount++;
			if (UpLeft == null || !UpLeft.Valid)
				clearCount++;
			if (UpRight == null || !UpRight.Valid)
				clearCount++;
			if (DownLeft == null || !DownLeft.Valid)
				clearCount++;
			if (DownRight == null || !DownRight.Valid)
				clearCount++;

			if (clearCount > 0) {
				BadNode = true;
				DisableConnections ();
			}
		}		
		/*if (!BadNode)
			SetColor (Color.blue);
		else
			SetColor (Color.red);*/
	}

	//Remove connections that connect to bad nodes
	public void CheckConnectionsPass2() {
		if (Up != null && Up.Node != null && Up.Node.BadNode) {
			Up.Valid = false;
			//Up.DrawLine ();
		}
		if (Down != null && Down.Node != null && Down.Node.BadNode) {
			Down.Valid = false;
			//Down.DrawLine ();
		}
		if (Left != null && Left.Node != null && Left.Node.BadNode) {
			Left.Valid = false;
			//Left.DrawLine ();
		}
		if (Right != null && Right.Node != null && Right.Node.BadNode) {
			Right.Valid = false;
			//Right.DrawLine ();
		}
		if (UpLeft != null && UpLeft.Node != null && UpLeft.Node.BadNode) {
			UpLeft.Valid = false;
			//UpLeft.DrawLine ();
		}
		if (UpRight != null && UpRight.Node != null && UpRight.Node.BadNode) {
			UpRight.Valid = false;
			//UpRight.DrawLine ();
		}
		if (DownLeft != null && DownLeft.Node != null && DownLeft.Node.BadNode) {
			DownLeft.Valid = false;
			//DownLeft.DrawLine ();
		}
		if (DownRight != null && DownRight.Node != null && DownRight.Node.BadNode) {
			DownRight.Valid = false;
			//DownRight.DrawLine ();
		}
	}

	//Disable all connections going from this this
	public void DisableConnections() {
		if (Up != null)
			Up.Valid = false;
		if (Down != null)
			Down.Valid = false;
		if (Left != null)
			Left.Valid = false;
		if (Right != null)
			Right.Valid = false;
		if (DownLeft != null)
			DownLeft.Valid = false;
		if (DownRight != null)
			DownRight.Valid = false;
		if (UpRight != null)
			UpRight.Valid = false;
		if (UpLeft != null)
			UpLeft.Valid = false;
	}

		
	//Raycast in all 8 directions to determine valid routes
	public void InitializeConnections(Grid grid) {
		bool valid = true;

		// LEFT
		if (X > 0) {
			valid = true;
			//Left
			if (grid.Nodes[X-1, Y].BadNode) {
				valid = false;
			}
			Left = new NodeConnection(this, grid.Nodes[X-1, Y], valid);

			//UpLeft
			if (Y < grid.Height-2) {
				valid = true;
				if (grid.Nodes[X-1, Y+1].BadNode) {
					valid = false;
				}
				UpLeft = new NodeConnection(this, grid.Nodes[X-1, Y+1], valid);
			}

			//DownLeft
			if (Y > 0) {
				valid = true;
				if (grid.Nodes[X-1, Y-1].BadNode) {
					valid = false;
				}			
				DownLeft = new NodeConnection(this,grid.Nodes[X-1, Y-1], valid);
			}
		}


		// RIGHT
		if (X < grid.Width - 2) {
			// Right
			valid = true;
			if (grid.Nodes[X+1, Y].BadNode) {
				valid = false;
			}
			Right = new NodeConnection(this,grid.Nodes[X+1, Y], valid);

			//UpRight
			if (Y < grid.Height -2) {
				valid = true;
				if (grid.Nodes[X+1, Y+1].BadNode) {
					valid = false;
				}
				UpRight = new NodeConnection(this,grid.Nodes[X + 1, Y + 1], valid);
			}

			//DownRight
			if (Y > 0) {
				valid = true;
				if (grid.Nodes[X+1, Y-1].BadNode) {
					valid = false;
				}
				DownRight = new NodeConnection(this,grid.Nodes[X+1, Y-1], valid);
			}

		}

		// VERTICAL
		if (Y  < grid.Height - 2) {
			// Up
			valid = true;
			if (grid.Nodes[X, Y+1].BadNode) {
				valid = false;
			}
			Up = new NodeConnection(this,grid.Nodes[X, Y+1], valid);
		}


		if (Y > 0) {
			// Down
			valid = true;
			if (grid.Nodes[X, Y-1].BadNode) {
				valid = false;
			}
			Down = new NodeConnection(this,grid.Nodes[X, Y-1], valid);
		}
	}


}
