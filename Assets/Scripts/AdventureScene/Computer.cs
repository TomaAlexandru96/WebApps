using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Entity {

	public bool exploded = false;
	private Object explosion;

	public void explode() {
		exploded = true;
		PlayAnimation ("PlayHackingAnimation");
	}

	protected override void SetStats() {
		this.stats = new ComputerStats (0, 10, 0);
	}

	/*protected override void OnSendNext (PhotonStream stream, PhotonMessageInfo info) {
		
	}

	protected override void OnReceiveNext (PhotonStream stream, PhotonMessageInfo info) {
		
	}*/

	protected IEnumerator PlayHackingAnimation() {
		GetComponent<Animator> ().Play ("Hacking");
		yield return new WaitForSeconds (0.9f);
		GetComponent<Animator> ().Play ("Off");
		explosion = Resources.Load ("Explosion");
		GameObject ob = NetworkService.GetInstance ().SpawnScene (explosion.name, transform.position, Quaternion.identity, 0);
		CircleCollider2D explosionRange = gameObject.AddComponent<CircleCollider2D> () as CircleCollider2D;
		explosionRange.isTrigger = true;
		explosionRange.radius = 1;
		Collider2D[] colls = new Collider2D[50];
		ContactFilter2D filter = new ContactFilter2D();
		filter.NoFilter ();
		explosionRange.OverlapCollider(filter, colls);
		foreach (Collider2D coll in colls) {
			if (coll == null) {
				break;
			}
			if (coll.gameObject.GetComponent<Player> () != null) {
				coll.gameObject.GetComponent<Player> ().GetHit (this);
			} else if (coll.gameObject.GetComponent<Enemy> () != null) {
				coll.gameObject.GetComponent<Enemy> ().GetHit (this);
			}

			Rigidbody2D rb = coll.gameObject.GetComponent<Rigidbody2D>();
			if (rb != null) {
				Vector2 dir = (transform.position - coll.transform.position);
				float wearoff = 1 - (Vector2.Distance(coll.transform.position, transform.position));
				rb.AddForce(dir.normalized * 30f * wearoff, ForceMode2D.Impulse);
			}
		}
		yield return new WaitForSeconds (0.9f);
		NetworkService.GetInstance().Destroy (ob);

		yield return GetEmptyIE ();
	}

}
