using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Entity : NetworkBehaviour {

	[SyncVar]
	public float curHP;
	[SyncVar]
	public float curSpeed;
	public EntityStats stats;

	protected void Start () {
		transform.SetParent (GameObject.FindGameObjectWithTag ("Grid").transform);
		SetStats ();
		curHP = stats.maxHP;
		curSpeed = stats.speed;
	}

	protected void Update () {
		if (isDead ()) {
			return;
		}

		Attack ();
		Move ();
	}

	protected virtual void Attack () {
		// used by children
	}

	protected virtual void Move () {
		// used by children
	}

	protected virtual void SetStats () {
		// used by children
	}

	public bool isDead () {
		return curHP <= 0;
	}

	public virtual void GetHit (Entity entity) {
		RpcPlayAnimation ("PlayGetHitAnimation");
	}

	protected void ChangeHealth (float newHealth) {
		curHP = Mathf.Clamp (newHealth, 0, stats.maxHP);
		if (isDead ()) {
			RpcPlayAnimation ("PlayDeadAnimation");
		}
	}

	// ----------------------------------------------------------------------------------------------------------
	// -------------------------------------------------SYNCH----------------------------------------------------
	// ----------------------------------------------------------------------------------------------------------

	/*#region IPunObservable implementation
	void IPunObservable.OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (curHP);
			stream.SendNext (curSpeed);
			OnSendNext (stream, info);

		} else {
			curHP = (float) stream.ReceiveNext ();
			curSpeed = (float) stream.ReceiveNext ();
			OnReceiveNext (stream, info);
		}
	}
	#endregion

	protected abstract void OnSendNext (PhotonStream stream, PhotonMessageInfo info);

	protected abstract void OnReceiveNext (PhotonStream stream, PhotonMessageInfo info);*/

	// ----------------------------------------------------------------------------------------------------------
	// ----------------------------------------------ANIMATIONS--------------------------------------------------
	// ----------------------------------------------------------------------------------------------------------

	[ClientRpc]
	protected void RpcPlayAnimation (string name) {
		StartCoroutine (name);
	}

	protected virtual IEnumerator PlayDeadAnimation () {
		yield return GetEmptyIE ();
	}

	protected virtual IEnumerator PlayGetHitAnimation () {
		yield return GetEmptyIE ();
	}

	protected IEnumerator GetEmptyIE () {
		yield return new WaitForSeconds (0f);
	}
}

