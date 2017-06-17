using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour {

	float time;
	private bool event1;
	private bool event2;
	private GameObject chatPanel;
	private GameObject playerAbilities;
	private GameObject directionPanel;
	private GameObject partMentionPanel;

	void Start () {
		time = Time.time;
		event1 = true;
		event2 = true;
		chatPanel = GameObject.FindGameObjectWithTag ("Chat");
		playerAbilities = GameObject.FindGameObjectWithTag ("PlayerAbilities");
		directionPanel = GameObject.FindGameObjectWithTag ("DirectionPanel");
		partMentionPanel = GameObject.FindGameObjectWithTag ("PartMention");
	}

	void Update () {

		if (event1 && Time.time > time) {
			event1 = false;
			chatPanel.SetActive (false);
			playerAbilities.SetActive (false);
			directionPanel.SetActive (false);
		}

		if (event2 && Time.time - time > 3) {
			event2 = false;
			partMentionPanel.SetActive (false);
			directionPanel.SetActive (true);
		}
	}

	public void ExitGame () {
		CurrentUser.GetInstance ().UnsubscribeCH (CurrentUser.GetInstance ().GetUserInfo ().party.owner);
		DBServer.GetInstance ().LeaveParty (CurrentUser.GetInstance ().GetUserInfo ().username, () => {
			SceneManager.LoadScene ("Menu");
		}, (error) => {
			Debug.LogError (error);	
		});
	}
}
