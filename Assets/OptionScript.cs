using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionScript : MonoBehaviour {

	private GameObject friendEntity;
	private string playerName;

	public GameObject Party;
	public GameObject OptionPanel;

	public void SetFriendEntity(GameObject friend) {
		friendEntity = friend;
	}

	private void UpdateInfo() {
		playerName = friendEntity.transform.GetChild (0).GetComponent<Text> ().text;
	} 

	public void InviteToParty() {
		UpdateInfo ();
		PartyMembers partyMembers = Party.transform.GetComponent<Party> ().getPartyMembers ();
		DBServer.GetInstance ().FindUser (playerName, (user) => {
			if (!partyMembers.ContainsPlayer (user.username) && user.active) {
				UpdateService.GetInstance ().SendUpdate (new string[]{user.username}, UpdateService.CreateMessage (UpdateType.PartyRequest));
			}
		}, (error) => {
			Debug.LogError (error);
		});
		OptionPanel.SetActive (false);
	}

	public void InviteToChat() {
		UpdateInfo ();
	}
}
