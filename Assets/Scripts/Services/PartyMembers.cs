using System;

public class PartyMembers {
	public string[] partyMembers = new string[Party.maxSize];
	public int index = 0;

	public void AddPlayer (string name) {
		partyMembers [index++] = name;
	}

	public int GetSize () {
		return index;
	}

	public string[] GetMembers () {
		string[] allMembers = new string[index];
		for (int i = 0; i < index; i++) {
			allMembers [i] = partyMembers [i];
		}
		return allMembers;
	}

	public void RemovePlayer (string name) {
		partyMembers [index--] = name;
	}
}

