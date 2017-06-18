using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourseworkScript : MonoBehaviour {

	private ArrayList questionLeft = new ArrayList ();

	public Text questionPanelText;
	public GameObject button0;
	public GameObject button1;
	public GameObject button2;
	public GameObject button3;
	public GameObject commonRoom;

	public int numberOfAvailableQuestions;
	public QuestionsAndAnswers[] qaa;
	public int numberOfQuestionsToAsk;
	public int questionsGotRight;
	public GameObject[] arrowsAcc = new GameObject[8];
	public GameObject[] arrowsDacc = new GameObject[8];

	private int numberOfThisQuestion;
	private int questionsAsked;
	private bool doneIntroduction;
	private bool acceptsInput;

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
		acceptsInput = true;
		doneIntroduction = false;
		qaa = new QuestionsAndAnswers[numberOfAvailableQuestions]; 
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
			questionPanelText.text = "This is your first Imperial coursework. It will consist of " + numberOfQuestionsToAsk + " questions that will mostly cover revision maths and some basic Discrete Maths" +
				" and Maths Methods";
			yield return new WaitForSeconds (3f);
			questionPanelText.text = "Press any key to start the coursework!";
			yield return new WaitForSeconds (1f);
			while (!Input.anyKey) yield return null;

			button0.SetActive (true);
			button1.SetActive (true);
			button2.SetActive (true);
			button3.SetActive (true);

			doneIntroduction = true;

			StartCoroutine (NextQuestion ());
		} else {

			if (questionsAsked < numberOfQuestionsToAsk) {
				ShowQuestion ();
			} else {
				button0.SetActive (false);
				button1.SetActive (false);
				button2.SetActive (false);
				button3.SetActive (false);
				questionPanelText.text = "The coursework is complete. You now need to go to the printers in labs, print your work, sign it and submit it to the Student Administration Office (SAO) \n " +
					" Quick, the deadling is approaching !!!";
				foreach (GameObject arrow in arrowsAcc) {
					arrow.SetActive (true);
				}
				foreach (GameObject arrow in arrowsDacc) {
					arrow.SetActive (false);
				}
				yield return new WaitForSeconds (5f);
				Debug.Log (questionsGotRight);
				commonRoom.GetComponent<CommonRoomScript> ().setQuestionsGotRight (questionsGotRight);
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


		acceptsInput = true;
		questionsAsked++;
	}

	private int PickQuestionNumber(){

		int index = (int)Random.Range (0f, (float)questionLeft.Count);
		int questionToAsk = (int)questionLeft [index];
		questionLeft.RemoveAt (index);
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
		if (acceptsInput) {
			if (answerNumber == qaa [numberOfThisQuestion].numberOfCorrectAnswer) {
				questionsGotRight++;
			} 	
			StartCoroutine (NextQuestion ());

		}
	}
		
	void InitializeQuestionsStruct () {
		for (int i = 0; i < numberOfAvailableQuestions; i++) {
			qaa [i].askedBefore = false;
		}

		//Make sure the number of questions you input is equal to the << numberOfAvailableQuestions>> variable


		CreateQuestion (0, "Which one of these is a valid convergence test ?", "D'Alambert's ratio test", "Einstein's ratio test", "Ibrahimovic's limit test", "All of the above", 0);
		CreateQuestion (1, "Which one of the following statements are true ?", "The geometric series diverges for any |X| < 2", "The geometric series diverges for any |X| > 2", "The harmonic series diverges", "All of the above", 2);
		CreateQuestion (2, "Assuming p is true and q is false, which one of these logical statements is valid ? ?", "q -> p", "q -> !p", "p -> p", "All of the above", 3);
		CreateQuestion (3, "Differentiate f(x) = sin(x)cos(x)", "2 sin(x)cos(x)", "-2 sin(x)cos(x)", "sin(2x)", "cox(2x)", 3);
		CreateQuestion (4, "What is the next number in the sequence : 0 3 8 15", "21", "24", "26", "35", 1);
		CreateQuestion (5, "Which one of the following is true ?", "every natural number n > 2 can be written as the sum of two prime numbers", "not every natural number n > 2 can be written as the sum of two prime numbers",
							"every even number is the difference of two primes", "we don't know the answer to any of that", 3);
		CreateQuestion (6, "What is the median of the following set : 1 2 4 6 10 13", "6", "5", "7", "8", 1);
		CreateQuestion (7, "What is the next number in the sequence : 1 1 2 3 5 8 13", "30", "27", "21", "25", 3);
		CreateQuestion (8, "Differentiate f(x) = ln(x) ^ 2", "2 ln(x) / x", " 2 ln(x)", " ln(x) / 2x", "3/2 ln(x)^3", 0);
		CreateQuestion (9, "What is the mean of the following set : 1 2 4 6 10 13", "6", "5", "7", "8", 3);

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




}

