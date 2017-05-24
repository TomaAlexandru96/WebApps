using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopUIPanelController : MonoBehaviour {

	public GameObject MenuPanel;
	public GameObject LeaderBoard;
	public GameObject PlayerInfo;

	/* Used by login button to change the pane to Menu pane */
	public void ActivateMenu () {
		MenuPanel.SetActive (true);
		LeaderBoard.SetActive (false);
		PlayerInfo.SetActive (false);
	}

	/* Used by login button to change the pane to LeaderBoard pane */
	public void ActivateLeaderBoard () {
		MenuPanel.SetActive (false);
		LeaderBoard.SetActive (true);
		PlayerInfo.SetActive (false);
	}

	/* Used by login button to change the pane to PlayerInfo pane */
	public void ActivatePlayerInfo () {
		MenuPanel.SetActive (false);
		LeaderBoard.SetActive (false);
		PlayerInfo.SetActive (true);
	}
}
