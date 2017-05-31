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
		for (int i = 0; i < index; i++) {
			if (partyMembers[i].Equals (name)) {
				partyMembers [i] = null;
				Shift (i);
				break;
			}
		}
		index--;
	}

	private void Shift (int i) {
		for (int q = i; q < index - 1; q++) {
			partyMembers [q] = partyMembers [q + 1];
		}
	}

	public void RemoveAllButOwner (string owner) {
		partyMembers = new string[Party.maxSize];
		index = 0;
		AddPlayer (owner);
	}
}

