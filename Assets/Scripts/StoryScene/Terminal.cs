using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Terminal : MonoBehaviour {

	public GameObject terminalEntry;
	public GameObject scrollView;
	public GameObject instructions;

	public GameObject terminal;
	public GameObject vim;
	public EventSystem eventSystem;
	public int numberOfEntries = 0;

	public int executionNumeber = 0;

	public void ExecuteCommand(Transform command) {
		string executable = command.GetComponent<InputField> ().text;
		executable += " ";
		int totalEntry = transform.GetChild (0).childCount;
		string[] execuatableList = executable.Split(new char[] {' '});


		if (executionNumeber==0 &&execuatableList [0].Equals ("vim") && execuatableList [1].Equals ("work.hs")) {
			terminal.SetActive (false);
			vim.SetActive (true);
			eventSystem.GetComponent<TerminalEventSystem> ().vimActive = true;
			executionNumeber++;
		} else if (executionNumeber == 1 &&execuatableList [0].Equals ("git") && execuatableList [1].Equals ("init")){
			executionNumeber++;
			CreateNewLine ();
			transform.GetChild (0).GetChild (totalEntry).GetChild(0).GetComponent<Text>().text = "Initialized empty Git repository";
		} else if (executionNumeber == 2 &&execuatableList [0].Equals ("git") && execuatableList [1].Equals ("remote") && execuatableList [2].Equals ("add")
			&& execuatableList [3].Equals ("origin") && execuatableList [4].Equals ("https://github.com/user/repo.git")){
			executionNumeber++;
		} else if (executionNumeber == 3 &&execuatableList [0].Equals ("git") && execuatableList [1].Equals ("add") && execuatableList [2].Equals ("work.hs")) {
			executionNumeber++;
			CreateNewLine ();
			transform.GetChild (0).GetChild (totalEntry).GetChild(0).GetComponent<Text>().text = "Added file work.hs";
		} else if (executionNumeber == 4 &&execuatableList [0].Equals ("git") && execuatableList [1].Equals ("commit")) {
			executionNumeber++;
			CreateNewLine ();
			transform.GetChild (0).GetChild (totalEntry).GetChild(0).GetComponent<Text>().text = "[master (root-commit) 39i8fp] add work.hs";
			CreateNewLine ();
			transform.GetChild (0).GetChild (totalEntry+1).GetChild(0).GetComponent<Text>().text = "1 file changed, 1 insertion(+)";
			CreateNewLine ();
			transform.GetChild (0).GetChild (totalEntry+2).GetChild(0).GetComponent<Text>().text = "create mode 100644 work.hs";
		} else if (executionNumeber == 5 &&execuatableList [0].Equals ("git") && execuatableList [1].Equals ("push")) {
			CreateNewLine ();
			transform.GetChild (0).GetChild (totalEntry).GetChild(0).GetComponent<Text>().text = "Counting objects: 1, done.";
			CreateNewLine ();
			transform.GetChild (0).GetChild (totalEntry+1).GetChild(0).GetComponent<Text>().text = "Delta compression using up to 4 threads.";
			CreateNewLine ();
			transform.GetChild (0).GetChild (totalEntry+2).GetChild(0).GetComponent<Text>().text = "remote: Resolving deltas: 100%, completed.";
			CreateNewLine ();
			transform.GetChild (0).GetChild (totalEntry+3).GetChild(0).GetComponent<Text>().text = "Successfully pushed to branch master";
			CreateNewLine ();
			transform.GetChild (0).GetChild (totalEntry+4).GetChild(0).GetComponent<Text>().text = "Press Enter to exit the terminal";
			executionNumeber++;
		} else if (executionNumeber == 6) {
			terminal.SetActive (false);
			instructions.SetActive (false);
		} else {
			Debug.Log ("try again");
		}
	}

	public void CreateNewLine() {
		GameObject entry = GameObject.Instantiate (terminalEntry, new Vector3(10f,160f-(20*numberOfEntries),0f), Quaternion.identity);
		entry.transform.SetParent (transform.GetChild(0), false);
		if (numberOfEntries > 10) {
			scrollView.GetComponent<ScrollRect> ().verticalNormalizedPosition = 0;
			scrollView.GetComponent<ScrollRect> ().velocity = new Vector2 (0f, 100f);
		}
		numberOfEntries++;
	}
		
}
