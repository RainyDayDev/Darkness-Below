using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dumb : MonoBehaviour {
    public Text level;
	// Use this for initialization
	void Start () {
        level.text = "YOU HAVE BEEN CONSUMED BY THE DARKNESS AT LEVEL "+PlayerPrefs.GetInt("HighestLevel");

	}
	
	
	
}
