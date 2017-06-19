using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public float spawnTime;        // The amount of time between each spawn.
	public GameObject[] enemy;

	public Transform target;
	public Transform myTransform;
	public float spawnRadius;

	void Awake() {
		myTransform = transform;
	}

	void Start() {
		spawnRadius = 1f;
	}

	public void Spawn (string[] enemies, int[] probab, int length, int enemiesNumber, float spawnTime) {
		StartCoroutine (SpawnHelper (enemies, probab, length, enemiesNumber, spawnTime));
	}

	private IEnumerator SpawnHelper (string[] enemies, int[] probab, int length, int enemiesNumber, float spawnTime) {
		for (int i = 0; i < enemiesNumber; i++) {
			// get random enemy
			int enemyP = Random.Range (0, 100);
			for (int p = 0; p < length; p++) {
				if (enemyP < probab[p]) {
					SpawnTimeDelay (enemies [p]);
					yield return new WaitForSeconds (spawnTime);
					break;
				}
			}
		}
	}

	void SpawnTimeDelay(string enemy) {
		float randomRadiusX = Random.Range (0.0f, 1.0f) * spawnRadius;
		float randomRadiusY = Random.Range (0.0f, 1.0f) * spawnRadius;
		NetworkService.GetInstance ().SpawnScene (
			enemy, 
			new Vector3(transform.position.x+randomRadiusX, transform.position.y+randomRadiusY, transform.position.z), 
			Quaternion.identity, 0);
	}
}
