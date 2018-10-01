﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Camera camera = FindObjectOfType<Camera> ();
		if (camera != null) {
			Destroy (gameObject);
		}
	}
	void Awake(){
		DontDestroyOnLoad (gameObject);
	}
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene ().name == "Tavern") {
			Destroy (gameObject);
		}
	}
}
