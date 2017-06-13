using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterviewScript : MonoBehaviour {

	private ArrayList questionLeft = new ArrayList ();

	public Text questionPanelText;
	public Text verdictPanel;
	public GameObject button0;
	public GameObject button1;
	public GameObject button2;
	public GameObject button3;

	public int numberOfAvailableQuestions;
	public QuestionsAndAnswers[] qaa;
	public int numberOfQuestionsToAsk;
	public int numberOfQuestionsToGetRight;

	public GameObject exitGate1;
	public GameObject exitGate2;
	public GameObject receptionist;

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
		button0.SetActive (false);
		button1.SetActive (false);
		button2.SetActive (false);
		button3.SetActive (false);
		InitializeQuestionsStruct ();
		StartCoroutine (NextQuestion ());
	}

	private IEnumerator NextQuestion () {


		if (!doneIntroduction) {
			questionPanelText.text = "Welcome to your Imperial Interview ! ";
			yield return new WaitForSeconds (3f);
			questionPanelText.text = "You will need to answer " + numberOfQuestionsToGetRight + " questions correctly to secure a place in Imperial !";
			yield return new WaitForSeconds (3f);
			questionPanelText.text = "Press any key to start the interview !";
			yield return new WaitForSeconds (1f);
			while (!Input.anyKey) yield return null;

			button0.SetActive (true);
			button1.SetActive (true);
			button2.SetActive (true);
			button3.SetActive (true);

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
					verdictPanel.text = "Oh, it looks like you still have some learning to do, don't worry, you can try again after you study a bit !";
				}
				yield return new WaitForSeconds (3f);
				verdictPanel.text = "";
				button0.SetActive (false);
				button1.SetActive (false);
				button2.SetActive (false);
				button3.SetActive (false);
				questionPanelText.text = "You have completed the first stage, you may now exit Huxley, go home and get some rest, the second stage will begin shortly !";
				yield return new WaitForSeconds (5f);
				Exit (exitGate1);
				Exit (exitGate2);
				Close ();
			}
		}
	}

	private void ShowQuestion () {
		numberOfThisQuestion = PickQuestionNumber ();
		questionPanelText.text = qaa [numberOfThisQuestion].question;

		button0.transform.GetChild(0).GetComponent<Text> ().text = qaa [numberOfThisQuestion].answer0;
		button1.transform.GetChild(0).GetComponent<Text> ().text  = qaa [numberOfThisQuestion].answer1;
		button2.transform.GetChild(0).GetComponent<Text> ().text = qaa [numberOfThisQuestion].answer2;
		button3.transform.GetChild(0).GetComponent<Text> ().text = qaa [numberOfThisQuestion].answer3;



		questionsAsked++;
	}

	private int PickQuestionNumber(){

		int index = (int)Random.Range (0f, (float)questionLeft.Count);
		int questionToAsk = (int)questionLeft [index];
		questionLeft.Remove (questionLeft.IndexOf(index));
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

		//Make sure the number of questions you input is equal to the << numberOfAvailableQuestions>> variable


		CreateQuestion (0, "What is the complexity of BubbleSort ?", "N", "N^2", "2^N", "NlogN", 1);
		CreateQuestion (1, "What is the complexity of MergeSort ?", "N", "N^2", "2^N", "NlogN", 3);
		CreateQuestion (2, "What is the binary representation of the number 32 ?", "00110011", "10000001", "11111110", "00100000", 3);
		CreateQuestion (3, "Which language would you use for low-level development ?", "C", "Java", "JavaScript", "C#", 0);
		CreateQuestion (4, "Which one of these is a functional language ?", "C#", "Java", "Haskell", "Ruby", 2);
		CreateQuestion (5, "You are given the following formula : ! (p AND q). Which one of the following formulas is equivalent to it ? " +
		" ( where ! is NOT)", "!p OR !q", "!(!p AND !q)", "p OR !q", "!p OR q", 0);
		CreateQuestion (6, "Convert 0010 1010 to decimal", "46", "32", "42", "66", 2);
		CreateQuestion (7, "Which is the best editor ?", "Gedit", "Notepad", "Vim", "Emacs", 2);
		CreateQuestion (8, "What is the integral of f(x) = ln(x) ?", "x^2", "ln(x)^2", "1 / ln(x)", "1 / x", 3);
		CreateQuestion (9, "Which one of these algorithms uses the Devide & Conquer technique ?", "Binary Search", "Dijkstra's Algorithm", "Bubble Sort", "KMP", 0);
		CreateQuestion (10, " (HARD!) What technique would you use to determine the longest common substring of 2 strings ?",
			"Divide & Conquer", "Dynammic Programming", "Greedy", "KMP", 1);
		CreateQuestion (11, "Differentiate f(x) = x ln (x)", "ln (x) + 1/x", "ln (x) + 1", "x^2 ln (x)", "2 * xln(x)", 1);
		CreateQuestion (12, "What is the equation of a circle with the center on the origin and a radius of 1 ? ", "x^2 + y^2 = 1", "x^2 / y + y^2 / x = 1", "x + y = 1", "x^3 + y^3 = 1", 0); 
	}

	private void CreateQuestion (int i,string question, string answer0, string answer1, 
								  string answer2, string answer3, int numberOfCorrectAnswer){

		questionLeft.Add (i);
		qaa [i].question = question;
		qaa [i].answer0 = answer0;
		qaa [i].answer1 = answer1;
		qaa [i].answer2 = answer2;
		qaa [i].answer3 = answer3;
		qaa [i].numberOfCorrectAnswer = numberOfCorrectAnswer;
	}

	private void Exit(GameObject exitGate) {
		exitGate.GetComponent<ExitGates> ().exit = true;
		receptionist.GetComponent<SpecifyMovementScript> ().ChangeRepeatingText ("receptionist: The exit is from the gates in the front!!");
	}


}

