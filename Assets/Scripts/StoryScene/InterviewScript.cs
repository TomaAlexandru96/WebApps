using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterviewScript : MonoBehaviour {


	public Text questionPanelText;
	public Text verdictPanel;
	public Text button0;
	public Text button1;
	public Text button2;
	public Text button3;

	public int numberOfAvailableQuestions;
	public QuestionsAndAnswers[] qaa;
	public int numberOfQuestionsToAsk;
	public int numberOfQuestionsToGetRight;

	private int numberOfThisQuestion;
	private int questionsAsked;
	private int questionsGotRight;


	public struct QuestionsAndAnswers{
		public string question;
		public string answer0;
		public string answer1;
		public string answer2;
		public string answer3;
		public int numberOfCorrectAnswer;
		public bool askedBefore;
	}


	public void Start () {
		questionsAsked = 0;
		questionsGotRight = 0;
		qaa = new QuestionsAndAnswers[2]; 
		InitializeQuestionsStruct ();
		NextQuestion ();
	}

	private void NextQuestion () {

		if (questionsAsked < numberOfQuestionsToAsk && questionsGotRight < numberOfQuestionsToGetRight) {
			ShowQuestion ();
		}else {
			if (questionsGotRight == numberOfQuestionsToGetRight) {
		//		verdictPanel.color = Color.green;
				verdictPanel.text = "Welcome to hell";
			} else {
			//	verdictPanel.color = Color.red;
				verdictPanel.text = "You no belong here";
			}
			StartCoroutine (Wait ());
			Close ();

		}
	}

	private void ShowQuestion () {
		numberOfThisQuestion = PickQuestionNumber ();
		questionPanelText.text = qaa [numberOfThisQuestion].question;

		button0.text = qaa [numberOfThisQuestion].answer0;
		button1.text = qaa [numberOfThisQuestion].answer1;
		button2.text = qaa [numberOfThisQuestion].answer2;
		button3.text = qaa [numberOfThisQuestion].answer3;

		questionsAsked++;
	}

	private int PickQuestionNumber(){

		bool ok = false;
		int questionToAsk = 0;

		while (!ok) {
			questionToAsk= (int) Random.Range (0f, (float)numberOfAvailableQuestions);
			if (qaa [numberOfThisQuestion].askedBefore == false)
				ok = true;
		}
		return questionToAsk;
	}
		
	public void Update () {
		if (Input.GetKeyUp (KeyCode.Escape)) {
			Close ();
		}
	}

	public void Close () {
		Destroy (gameObject);
	}



	public void Verify (int answerNumber) {
		bool correct = false;
		if (answerNumber == qaa [numberOfThisQuestion].numberOfCorrectAnswer) {
			correct = true;
			questionsGotRight++;
		} 		
		DisplayMessage (correct);
		NextQuestion ();
	}
		
	void DisplayMessage (bool correct) {
		if (correct) {
		//	verdictPanel.color = Color.green;
			verdictPanel.text = "You're a genius";
		} else {
		//	verdictPanel.color = Color.red;
			verdictPanel.text = "you stupid";
		}
		StartCoroutine (Wait ());
		verdictPanel.text = "";
	}

	private IEnumerator Wait () {
		yield return new WaitForSeconds (3f);
	}

	void InitializeQuestionsStruct () {
		for (int i = 0; i < numberOfAvailableQuestions; i++) {
			qaa [i].askedBefore = false;
		}

		qaa [0].question = "ce fa nec?";
		qaa [0].answer0 = "bine tiu";
		qaa [0].answer1 = "sugi pl";
		qaa [0].answer2 = "sugi pizda";
		qaa [0].answer3 = "sugi curu";
		qaa [0].numberOfCorrectAnswer = 0;

		qaa [1].question = "ce faci nec?";
		qaa [1].answer0 = "sugi";
		qaa [1].answer1 = "bugi";
		qaa [1].answer2 = "bine tiu";
		qaa [1].answer3 = "fugi";
		qaa [1].numberOfCorrectAnswer = 2;
				
	}
}

