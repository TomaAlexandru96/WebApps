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
	public GameObject attackRadius;

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
		InvokeRepeating ("GetHitOvertime", 1, 30);
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
			Attack ();
			Move ();
		} else {
			move = Direction.Dead;
		}

		Animate ();
	}

	private Vector2 GetMouseInput () {
		RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		return (new Vector3 (hit.point.x, hit.point.y, transform.position.z) - transform.position).normalized;
	}

	private void Attack () {
		/*// GET ATTACK INPUT
		if (Input.GetMouseButtonDown(0)) {
			Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pz.z = 0;
			if (weapon.longRange) {
				// for now do nothing
			}
		}*/

		if (Input.GetKeyDown (KeyCode.Space)) {
			Vector3 mouseDirection = GetMouseInput ();

			bool inverted = Vector3.Cross (attackRadius.transform.localPosition, mouseDirection).z < 0;
			float angle = Vector3.Angle (attackRadius.transform.localPosition, mouseDirection);
			angle = inverted ? -angle : angle;
			attackRadius.transform.RotateAround (transform.position, Vector3.forward, angle);
		}
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
		
	public void GetHitOvertime () {
		curHP -= 0.5f;
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

