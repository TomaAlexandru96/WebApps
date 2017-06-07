using UnityEngine;
using System.Collections;

public class NodeConnection {
	
	public Node Parent;
	public Node Node;
	public bool Valid;

	//Debug
	GameObject draw;

	public NodeConnection(Node parent, Node node, bool valid) {
		Valid = valid;
		Node = node;
		Parent = parent;

		if (Node != null && Node.BadNode) {
			Valid = false;
		}

		if (Parent != null && Parent.BadNode) {
			Valid = false;
		}
		//DrawLine ();
	}

	//Debug
	public void DrawLine()
	{	
		/*if (Parent != null && Node != null) {
			draw = GameObject.Instantiate (Resources.Load ("Line")) as GameObject;
			LineRenderer lr = draw.GetComponent<LineRenderer> ();

			lr.SetPosition (0, Parent.Position);
			lr.SetPosition (1, Node.Position);
			if (Valid) {
				lr.startColor = Color.green;
				lr.endColor = Color.green;
			} else {
				lr.startColor = Color.red;
				lr.endColor = Color.red;
			}
		}*/
	}
}
