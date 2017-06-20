using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderboardController : MonoBehaviour {

	public GameObject leaderEntry;
	public GameObject content;
	public MenuController menu;

	void Start () {
		InvokeRepeating ("Refresh", 0f, 100f);
	}

	public void Exit () {
		menu.SwtichToMenuView ();
	}

	public void Refresh () {
		Clear ();

		DBServer.GetInstance ().GetLeaderboard ((leaderboard) => {
			int i = 1;
			foreach (User le in leaderboard.users) {
				GameObject go = Instantiate (leaderEntry);
				go.GetComponent<LeaderboardElement> ().Init (i, le);
				go.transform.SetParent (content.transform);
				i++;
			}	
		}, (error) => {
			Debug.Log (error);
		});
	}

	public void Clear () {
		foreach (Transform child in content.transform) {
			Destroy (child.gameObject);
		}
	}
}
