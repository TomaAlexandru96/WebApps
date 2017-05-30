using System;

public class PartyMembers {
	public string[] partyMembers = new string[PartyControl.maxSize];
	public int index = 0;

	public void AddPlayer (string name) {
		partyMembers [index++] = name;
	}

	public int GetSize () {
		return index;
	}
}

