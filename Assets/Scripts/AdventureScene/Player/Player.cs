using System;
using System.Collections;
using UnityEngine;

public class Player : Entity<PlayerStats> {
	
	public Direction move;
	public Camera mainCamera;

	public GameObject attackRadius;

	protected string username;
	protected PlayerAbilities abilities;

	void Awake () {
		this.username = (string) photonView.instantiationData [0];
	}

	new void Start () {
		base.Start ();
		mainCamera.enabled = photonView.isMine;
		mainCamera.GetComponent<AudioListener> ().enabled = photonView.isMine;
		InvokeRepeating ("GetHitOvertime", 10, 20);

		if (photonView.isMine) {
			abilities = GameObject.FindGameObjectWithTag ("PlayerAbilities").GetComponent<PlayerAbilities> ();
			abilities.Init (this);
		}
	}

	protected override IEnumerator PlayDeadAnimation () {
		move = Direction.Dead;
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		photonView.RPC ("Animate", PhotonTargets.All);
		yield return GetEmptyIE ();
	}

	protected override void SetStats () {
		stats = new PlayerStats (PlayerType.FrontEndDev);
	}

	private bool IsStory () {
		return CurrentUser.GetInstance ().GetUserInfo ().party.state == PartyMembers.STORY;
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

		curSpeed = stats.speed;
		if (Input.GetKey (KeyCode.LeftShift)) {
			if (abilities.Sprint ()) {
				curSpeed = stats.runSpeed;
			} else {
				curSpeed = stats.speed;
			}
		}
	}

	protected override void Attack () {
		SelectAbility ();

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
				StartCoroutine (PlayMeleAttackAnimation ());
			} else {
				Debug.LogWarning ("Not yet implemented: " + selectedAbility.ToString ());
			}
		}
	}

	protected IEnumerator PlayMeleAttackAnimation () {
		attackRadius.GetComponent<Animator> ().Play ("Slash");
		attackRadius.GetComponent<PlayerAttack> ().StartAttack ();
		yield return new WaitForSeconds (0.1f);
		attackRadius.GetComponent<Animator> ().Play ("Default");
		attackRadius.GetComponent<PlayerAttack> ().StopAttack ();
	}

	protected override void Move () {
		// GET MOVEMENT INPUT
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		// MOVEMENT
		Vector2 movement = new Vector2 (h, v).normalized;
		GetComponent<Rigidbody2D> ().velocity = movement * curSpeed;
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
		photonView.RPC ("Animate", PhotonTargets.All);
	}
		
	public void GetHitOvertime () {
		ChangeHealth (curHP - 1);
	}

	public void GetBuff (Buff buff) {
		if (buff == Buff.Coffee) {
			IncreaseHealth(10f);
		}
	}

	public void IncreaseHealth (float points) {
		ChangeHealth (curHP + points);
	}

	protected override IEnumerator PlayGetHitAnimation () {
		GetComponent<SpriteRenderer> ().color = UnityEngine.Color.red;
		yield return new WaitForSeconds (0.1f);
		GetComponent<SpriteRenderer> ().color = UnityEngine.Color.white;
	}

	public override void GetHit<E> (Entity<E> entity) {
		ChangeHealth (curHP - entity.stats.damage);
		base.GetHit (entity);
	}

	public string GetName () {
		return username;
	}

	[PunRPC]
	protected virtual void Animate () {
		// used by children
	}

	protected override void OnSendNext (PhotonStream stream, PhotonMessageInfo info) {
		stream.SendNext (move);
	}

	protected override void OnReceiveNext (PhotonStream stream, PhotonMessageInfo info) {
		move = (Direction) stream.ReceiveNext ();
	}
}

