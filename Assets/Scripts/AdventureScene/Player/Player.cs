using System;
using System.Collections;
using UnityEngine;

public class Player : Photon.PunBehaviour, IPunObservable {
	
	public Direction move;
	public PlayerStats stats;
	public Camera mainCamera;

	public bool dead = false;
	public float curHP;

	public Inventory inventory;
	public Item weapon;
	public GameObject attackRadius;

	protected Rigidbody2D rb;
	protected Animator animator;
	protected string username;
	protected PlayerAbilities abilities;

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
		InvokeRepeating ("GetHitOvertime", 10, 20);

		if (photonView.isMine) {
			abilities = GameObject.FindGameObjectWithTag ("PlayerAbilities").GetComponent<PlayerAbilities> ();
			abilities.Init (this);
		}
	}

	void Update () {
		if (!photonView.isMine || ChatController.GetChat ().IsFocused ()) {
			return;
		}

		if (!dead) {
			SelectAbility ();
			Attack ();
			Move ();
		} else {
			move = Direction.Dead;
		}

		Animate ();
	}

	private Vector2 GetMouseInput () {
		RaycastHit2D hit = Physics2D.Raycast (mainCamera.ScreenToWorldPoint (Input.mousePosition), 
			Vector2.zero, 1f, LayerMask.GetMask (new string[] {"MouseInput"}));
		return (new Vector3 (hit.point.x, hit.point.y, transform.position.z) - transform.position).normalized;
	}

	private void SelectAbility () {
		if (Input.GetKeyUp (KeyCode.Alpha1)) {
			abilities.SelectAbility (1);
		} else if (Input.GetKeyUp (KeyCode.Alpha2)) {
			abilities.SelectAbility (2);
		} else if (Input.GetKeyUp (KeyCode.Alpha3)) {
			abilities.SelectAbility (3);
		} else if (Input.GetKeyUp (KeyCode.Alpha4)) {
			abilities.SelectAbility (4);
		}
	}

	private void Attack () {
		if (Input.GetKeyUp (KeyCode.Space)) {
			if (!abilities.UseAbility ()) {
				return;
			}

			Ability selectedAbility = abilities.GetSelectedAbility ();

			if (selectedAbility.type == Ability.Mele) {
				Vector3 mouseDirection = GetMouseInput ();

				bool inverted = Vector3.Cross (attackRadius.transform.localPosition, mouseDirection).z < 0;
				float angle = Vector3.Angle (attackRadius.transform.localPosition, mouseDirection);
				angle = inverted ? -angle : angle;
				attackRadius.transform.RotateAround (transform.position, Vector3.forward, angle);
				StartCoroutine (PlayAttackAnimation ());	
			} else {
				Debug.LogWarning ("Not yet implemented: " + selectedAbility.ToString ());
			}
		}
	}

	private IEnumerator PlayAttackAnimation () {
		attackRadius.GetComponent<Animator> ().Play ("Slash");
		attackRadius.GetComponent<PlayerAttack> ().StartAttack ();
		yield return new WaitForSeconds (0.1f);
		attackRadius.GetComponent<Animator> ().Play ("Default");
		attackRadius.GetComponent<PlayerAttack> ().StopAttack ();
	}

	private void Move () {
		// GET MOVEMENT INPUT
		float h = Input.GetAxisRaw ("Horizontal");

		float v = Input.GetAxisRaw ("Vertical");

		// MOVEMENT
		Vector2 movement = new Vector2 (h, v).normalized;
		rb.velocity = movement * stats.speed;

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
		StartCoroutine (PlayGetHitAnimation ());
		curHP -= enemy.stats.damage;
		if (curHP <= 0) {
			curHP = 0;
			dead = true;
		}
	}
		
	public void GetHitOvertime () {
		curHP -= 1f;
	}

	public void GetBuff (Buff buff) {
		if (buff == Buff.Coffee) {
			IncreaseHealth(10);
		}
	}

	public void IncreaseHealth (float points) {
		curHP = Mathf.Clamp (points + curHP, 0, stats.maxHP);
	}

	private IEnumerator PlayGetHitAnimation () {
		GetComponent<SpriteRenderer> ().color = UnityEngine.Color.red;
		yield return new WaitForSeconds (0.1f);
		GetComponent<SpriteRenderer> ().color = UnityEngine.Color.white;
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

