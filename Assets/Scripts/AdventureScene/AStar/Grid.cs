using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public Vector2 Offset;

	public int Width;
	public int Height;

	public Node[,] Nodes;

	public int Left { get { return 0; } }
	public int Right { get { return Width; } }
	public int Down { get { return 0; } }
	public int Up { get { return Height; } }

	public const float UnitSize = 1f;

	GameObject Enemy;
	private LineRenderer LineRenderer;

	void Awake () {	
		Enemy = GameObject.FindGameObjectWithTag ("Enemy");

		//Get grid dimensions
		Offset = this.transform.position;

		Width = (int) GetComponent<RectTransform> ().rect.width * 2+ 2;
		Height = (int) GetComponent<RectTransform> ().rect.height * 2+ 2;

		Debug.Log (Width);
		Debug.Log (Height);

		Nodes = new Node[Width, Height];

		// Initialize the grid nodes - 1 grid unit between each node
		// We render the grid in a diamond pattern
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				//float offset = 0;
				//if (y%2 != 0) {
				//	offset = 0.25f;
				//}
		
				Vector2 pos = new Vector2(((x*0.5f))+transform.position.x, (y*0.5f)+transform.position.y);
				Node node = new Node(x, y, pos, this);
				Nodes[x, y] = node;

			}
		}

		//Create connections between each node
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				if (Nodes[x,y].BadNode) continue;
				Nodes[x, y].InitializeConnections(this);
			}
		}		

		//Pass 1, we removed the bad nodes, based on valid connections
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				if (Nodes [x, y] == null || Nodes[x,y].BadNode) {
					continue;				
				}
				Nodes[x, y].CheckConnectionsPass1 (this);
			}
		}		

		//Pass 2, remove bad connections based on bad nodes
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				if (Nodes [x, y] == null) {
					continue;				
				}

				Nodes[x, y].CheckConnectionsPass2 ();
			}
		}
	}		

	public bool ConnectionIsValid(Point point1, Point point2) {
		//comparing same point, return false
		if (point1.X == point2.X && point1.Y == point2.Y)
			return false;

		if (Nodes [point1.X, point1.Y] == null)
			return false;

		//determine direction from point1 to point2
		Direction direction = Direction.Down;

		if (point1.X == point2.X) {
			if (point1.Y < point2.Y)
				direction = Direction.Down;
			else if (point1.Y > point2.Y)
				direction = Direction.Up;
		} else if (point1.Y == point2.Y) {
			if (point1.X < point2.X)
				direction = Direction.Right;
			else if (point1.X > point2.X)
				direction = Direction.Left;
		} else if (point1.X < point2.X) {
			if (point1.Y > point2.Y)
				direction = Direction.UpRight;
			else if (point1.Y < point2.Y)
				direction = Direction.DownRight;
		} else if (point1.X > point2.X) {
			if (point1.Y > point2.Y)
				direction = Direction.UpLeft;
			else if (point1.Y < point2.Y)
				direction = Direction.DownLeft;
		}

		//check connection
		switch (direction) {
		case Direction.Down:
			if (Nodes[point1.X, point1.Y].Down != null)
				return Nodes[point1.X, point1.Y].Down.Valid;
			else
				return false;

		case Direction.Up:
			if (Nodes[point1.X, point1.Y].Up != null)
				return Nodes[point1.X, point1.Y].Up.Valid;
			else
				return false;

		case Direction.Right:
			if (Nodes[point1.X, point1.Y].Right != null)
				return Nodes[point1.X, point1.Y].Right.Valid;
			else
				return false;

		case Direction.Left:
			if (Nodes[point1.X, point1.Y].Left != null)
				return Nodes[point1.X, point1.Y].Left.Valid;
			else
				return false;

		case Direction.DownLeft:
			if (Nodes[point1.X, point1.Y].DownLeft != null)
				return Nodes[point1.X, point1.Y].DownLeft.Valid;
			else
				return false;

		case Direction.DownRight:
			if (Nodes[point1.X, point1.Y].DownRight != null)
				return Nodes[point1.X, point1.Y].DownRight.Valid;
			else
				return false;

		case Direction.UpLeft:
			if (Nodes[point1.X, point1.Y].UpLeft != null)
				return Nodes[point1.X, point1.Y].UpLeft.Valid;
			else
				return false;

		case Direction.UpRight:
			if (Nodes[point1.X, point1.Y].UpRight != null)
				return Nodes[point1.X, point1.Y].UpRight.Valid;
			else
				return false;

		default:
			return false;
		}		
	}

	public void DrawPath (BreadCrumb bc) {
		//Vector2 target = GameObject.FindGameObjectWithTag ("Player").transform.localPosition;
		//Point gridPos = new Point((int)(target.x*2), (int)(target.y*2));


		//if (gridPos != null) {						

			//if (gridPos.X > 0 && gridPos.Y > 0 && gridPos.X < Width && gridPos.Y < Height) {

				//Point enemyPos = new Point((int)(Enemy.transform.localPosition.x*2), (int)(Enemy.transform.localPosition.y*2));
				//Nodes [enemyPos.X, enemyPos.Y].SetColor (Color.blue);

				//BreadCrumb bc = PathFinder.FindPath (this, enemyPos, gridPos);

				int count = 0;
				LineRenderer lr = Enemy.GetComponent<LineRenderer> ();
				lr.positionCount = 100;
				lr.startColor = Color.yellow;
				lr.endColor = Color.yellow;

				//Draw out our path
				while (bc != null) {
					Vector2 bcRealPos = bc.toRealCoordinates (this);
					lr.SetPosition(count, new Vector3(bcRealPos.x, bcRealPos.y, 0));
					bc = bc.next;
					count += 1;
				}
				lr.positionCount = count;
			}
		//}
	//}

	void Start () {
		Vector2 target = GameObject.FindGameObjectWithTag ("Player").transform.localPosition;
		Point gridPos = new Point((int)(target.x*2), (int)(target.y*2));
		Debug.Log ("Player position x:" + gridPos.X + ", y: " + gridPos.Y);


		if (gridPos != null) {						

			if (gridPos.X > 0 && gridPos.Y > 0 && gridPos.X < Width && gridPos.Y < Height) {

				Point enemyPos = new Point((int)(Enemy.transform.localPosition.x*2), (int)(Enemy.transform.localPosition.y*2));
				Debug.Log ("Enemy position x:" + enemyPos.X + ", y: " + enemyPos.Y);

				Nodes [enemyPos.X, enemyPos.Y].SetColor (Color.blue);

				BreadCrumb bc = PathFinder.FindPath (this, enemyPos, gridPos);

				int count = 0;
				LineRenderer lr = Enemy.GetComponent<LineRenderer> ();
				lr.positionCount = 100;
				lr.startColor = Color.yellow;
				lr.endColor = Color.yellow;

				//Draw out our path
				while (bc != null) {
					float posX = (bc.position.X*0.5f) + transform.position.x;
					float posY = (bc.position.Y*0.5f) + transform.position.y;
					lr.SetPosition(count, new Vector3(posX, posY, 0));
					bc = bc.next;
					count += 1;
				}
				lr.positionCount = count;
			}
		}
	}

}
