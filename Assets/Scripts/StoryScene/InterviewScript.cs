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
	private bool doneIntroduction = false;


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
		qaa = new QuestionsAndAnswers[numberOfAvailableQuestions]; 
		verdictPanel.text = "";
		questionPanelText.text = "";
		InitializeQuestionsStruct ();
		StartCoroutine (NextQuestion ());
	}
		
	private IEnumerator NextQuestion () {


		if (!doneIntroduction) {
			questionPanelText.text = "Welcome to your Imperial Interview ! ";
			yield return new WaitForSeconds (3f);
			questionPanelText.text = "You will have to get" + numberOfQuestionsToGetRight + " questions right" + 
									 ", from the total of " + numberOfQuestionsToAsk + "to secure" + 
									 " your place in Imperial";
			yield return new WaitForSeconds (3f);
			doneIntroduction = true;
			StartCoroutine (NextQuestion ());
			
		} else {

			if (questionsAsked < numberOfQuestionsToAsk && questionsGotRight < numberOfQuestionsToGetRight) {
				ShowQuestion ();
			} else {
				if (questionsGotRight == numberOfQuestionsToGetRight) {
					verdictPanel.color = Color.green;
					verdictPanel.text = "Congratulations, welcome to Imperial";
				} else {
					verdictPanel.color = Color.red;
					verdictPanel.text = "Try again next year";
				}
				yield return new WaitForSeconds (3f);
				Close ();
			}
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
		StartCoroutine(DisplayMessage (correct));
	}

	private IEnumerator DisplayMessage (bool correct) {
		if (correct) {
			verdictPanel.color = Color.green;
			verdictPanel.text = "CORRECT";
		} else {
			verdictPanel.color = Color.red;
			verdictPanel.text = "INCORRECT";
		}

		yield return new WaitForSeconds (2f);

		verdictPanel.text = "";
		StartCoroutine (NextQuestion ());
	}

	void InitializeQuestionsStruct () {
		for (int i = 0; i < numberOfAvailableQuestions; i++) {
			qaa [i].askedBefore = false;
		}

		qaa [0].question = "Complexity of BubbleSort ?";
		qaa [0].answer0 = "N";
		qaa [0].answer1 = "N^2";
		qaa [0].answer2 = "2^N";
		qaa [0].answer3 = "NlogN";
		qaa [0].numberOfCorrectAnswer = 1;

		qaa [1].question = "Complexity of MergeSort";
		qaa [1].answer0 = "N";
		qaa [1].answer1 = "N^2";
		qaa [1].answer2 = "2^N";
		qaa [1].answer3 = "NlogN";
		qaa [1].numberOfCorrectAnswer = 3;

		qaa [2].question = "32 in binary";
		qaa [2].answer0 = "00110011";
		qaa [2].answer1 = "10000001";
		qaa [2].answer2 = "11111110";
		qaa [2].answer3 = "00100000";
		qaa [2].numberOfCorrectAnswer = 3;


	}
}

