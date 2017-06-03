using System;
using UnityEngine;

[System.Serializable]
public class PartyMembers {
	
	public string[] partyMembers;
	public string owner;
	public int state;

	public int GetSize () {
		return partyMembers.Length;
	}

	public bool ContainsPlayer (string player) {
		if (partyMembers == null) {
			return false;
		}

		for (int q = 0; q < GetSize (); q++) {
			if (partyMembers [q].Equals (player)) {
				return true;
			}
		}
		return false;
	}

	public override string ToString () {
		if (partyMembers == null) {
			return "Not in party";
		}

		return "[owner: "+owner+", members: "+partyMembers.ToStringFull ()+"]";
	}
}

