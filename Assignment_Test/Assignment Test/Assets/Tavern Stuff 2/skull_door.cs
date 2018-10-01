using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class skull_door : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player") {
			SceneManager.LoadScene ("MainDungeon");
		}


	}
	// Update is called once per frame
	void Update () {
		
	}
}
