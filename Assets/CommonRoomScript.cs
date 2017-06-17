using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonRoomScript : MonoBehaviour {

	public GameObject courseworkPanel;
	public bool doneCoursework;
	public GameObject directionPanel;


	private int quesionsGotright;
	private int totalNumberOfQuestions;

	public void Start () {
		totalNumberOfQuestions = courseworkPanel.GetComponent<CourseworkScript> ().numberOfQuestionsToAsk;
		quesionsGotright = courseworkPanel.GetComponent<CourseworkScript> ().questionsGotRight;
		courseworkPanel.SetActive (false);
		doneCoursework = false;
		directionPanel.SetActive (false);
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (!doneCoursework) {
			courseworkPanel.SetActive (true);
		} else {
			
			directionPanel.SetActive (true);
			directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText ("You got " + quesionsGotright + " questions right, out of the total of " + totalNumberOfQuestions + "" +
				" that means your grade is " + CalculateGrade (quesionsGotright, totalNumberOfQuestions));
		}
	}
	void OnTriggerExit2D(Collider2D coll){
		doneCoursework = true;
	}

	private string CalculateGrade (int questionsGotRight, int totalNumberOfQuestions) {
		float fraction = (float)questionsGotRight / totalNumberOfQuestions;
		if (fraction > 0.9f)
			return "A*";
		else if (fraction > 0.8f)
			return "A+";
		else if (fraction > 0.7f)
			return "A";
		else if (fraction > 0.6f)
			return "B";
		else if (fraction > 0.5f)
			return "C";
		else if (fraction > 0.4f)
			return "D";
		else 
			return "F";
	}

}
