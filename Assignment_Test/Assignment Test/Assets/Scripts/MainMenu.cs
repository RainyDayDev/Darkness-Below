using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Button play_button;
	public Button howtoplay_button;
	public Button about_button;

	// Use this for initialization
	void Start () {
		play_button = play_button.GetComponent<Button> ();
		howtoplay_button = howtoplay_button.GetComponent<Button> ();
		about_button = about_button.GetComponent<Button> ();
	}
	
	public void play(){
		SceneManager.LoadScene ("Tavern");
	}

	public void about(){
		SceneManager.LoadScene ("about");
	}

	public void howtoplay(){
		SceneManager.LoadScene ("howtoplay");
	}

	public void exit(){
		Application.Quit ();
	}
}
