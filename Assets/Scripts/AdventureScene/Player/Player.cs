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

		if (!dead) {
			// GET ATTACK INPUT
			if (Input.GetMouseButtonDown(0)) {
				Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pz.z = 0;
				if (weapon.longRange) {
					// for now do nothing
				}
			}

			Move ();
		} else {
			move = Direction.Dead;
		}

		Animate ();
	}

	private void Move () {
		// GET MOVEMENT INPUT
		float h = Input.GetAxisRaw ("Horizontal");

		float v = Input.GetAxisRaw ("Vertical");

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
	}

	public void GetHit (Enemy enemy) {
		GetComponent<SpriteRenderer> ().color = UnityEngine.Color.red;
		startAttack = Time.time;
		curHP -= enemy.stats.damage;
		if (curHP <= 0) {
			curHP = 0;
			dead = true;
		}
	}

	private void HitEnemy (GameObject enemy) {
		enemy.transform.GetComponent <Enemy> ().GetHit (this);
	}

	#region IPunObservable implementation
	void IPunObservable.OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (move);
			stream.SendNext (curHP);
		} else {
			move = (Direction) stream.ReceiveNext ();
			curHP = (float)  stream.ReceiveNext ();
			Animate ();
		}
	}
	#endregion

	public string GetName () {
		return username;
	}

	protected virtual void Animate () { }
}

