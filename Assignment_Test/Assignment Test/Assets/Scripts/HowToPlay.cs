using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlay : MonoBehaviour {

	public Button return_button;


	// Use this for initialization
	void Start () {
		return_button = return_button.GetComponent<Button> ();
	}

	public void returning(){
		SceneManager.LoadScene ("Main Menu");
	}
		


	public void exit(){
		Application.Quit ();
	}
}