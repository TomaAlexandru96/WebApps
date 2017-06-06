using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public float spawnTime;        // The amount of time between each spawn.
	public GameObject[] enemy;

	public int maxDistance;
	public int minDistance;
	public Transform target;
	public Transform myTransform;

	void Awake() {
		myTransform = transform;
	}

	void Start() {
		GameObject stop = GameObject.FindGameObjectWithTag("Player");
		enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		target = stop.transform;
		maxDistance = 20;
		minDistance = 20;
		spawnTime = 10f;
		StartCoroutine(SpawnTimeDelay());
	}

	IEnumerator SpawnTimeDelay() {
		while (true) {
			float distance = Vector3.Distance (target.position, myTransform.position);
			if (distance < maxDistance && distance > minDistance) {
				int spawnPointIndex = Random.Range (0, enemy.Length);
				Instantiate (enemy[spawnPointIndex], transform.position, Quaternion.identity);
				yield return new WaitForSeconds (spawnTime);
			}

			if (Vector3.Distance (target.position, myTransform.position) > maxDistance) {
				yield return null;
			}                
		}
	}
}
