using System;

using UnityEngine;

public class Player : Photon.PunBehaviour, IPunObservable {

	public float speed;
	public Direction move;
	public PlayerStats stats;
	public Camera mainCamera;

	public bool dead = false;
	public float curHP;

	public Inventory inventory;
	public Item weapon;
	public float startAttack;

	protected Rigidbody2D rb;
	protected Animator animator;
	protected string username;

	void Awake () {
		this.username = (string) photonView.instantiationData [0];
	}

	void Start () {
		mainCamera.enabled = photonView.isMine;
		mainCamera.GetComponent<AudioListener> ().enabled = photonView.isMine;
		transform.SetParent (GameObject.FindGameObjectWithTag ("Grid").transform);
		rb = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator>();
		stats = new PlayerStats (PlayerType.FrontEndDev);
		curHP = stats.maxHP;
		weapon = new Item ("Sword", 3, 2, false);	
	}


	void Update () {
		if (!photonView.isMine || ChatController.GetChat ().IsFocused ()) {
			return;
		}

		if (startAttack + 0.5 < Time.time) {
			startAttack = Time.time;
			GetComponent<SpriteRenderer> ().color = UnityEngine.Color.white;
		}

		float h;
		float v;

		if (!dead) {

			// GET ATTACK INPUT
			if (Input.GetMouseButtonDown(0)) {
				Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pz.z = 0;
				if (weapon.longRange) {
					// for now do nothing
				}
			}

			// GET MOVEMENT INPUT
			h = Input.GetAxisRaw ("Horizontal");

			v = Input.GetAxisRaw ("Vertical");

			// MOVEMENT
			Vector2 movement = new Vector2 (h, v).normalized;
			rb.velocity = movement * speed;

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
		} else {
			move = Direction.Dead;
		}

		Animate ();

	}

	public void Damaged() {
		GetComponent<SpriteRenderer> ().color = UnityEngine.Color.red;
		startAttack = Time.time;
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "Enemy") {
			if (Input.GetMouseButtonDown(0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if(hit.collider != null) {
					HitEnemy (hit.collider.gameObject);
				}
			}
		}
	}

	private void HitEnemy(GameObject enemy) {
		switch (enemy.name) {
		case "EnemyGit":
			enemy.transform.GetComponent <Enemy> ().GetHit (stats.git);
			break;
		case "EnemyJS":
			enemy.transform.GetComponent <Enemy> ().GetHit (stats.javascript);
			break;
		}

	}

	#region IPunObservable implementation
	void IPunObservable.OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (move);
			stream.SendNext (curHP);
		} else {
			move = (Direction) stream.ReceiveNext ();
			curHP = (float) stream.ReceiveNext ();
			Animate ();
		}
	}
	#endregion

	public string GetName () {
		return username;
	}

	protected virtual void Animate () { }
}

