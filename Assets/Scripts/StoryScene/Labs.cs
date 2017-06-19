using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labs : MonoBehaviour {

	private GameObject students;

	void Update() {
		if (students == null) {
			students = GameObject.FindGameObjectWithTag("Students");
		}
	}

	public void RemoveStudents() {
		students.SetActive (false);
	}

	public void AddStudents() {
		students.SetActive (true);
	}
}
