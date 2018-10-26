using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonDescription : MonoBehaviour {
	public Text left;
	public Text right;
	public Text middle;
	Player player;
	// Use this for initialization
	void Start () {
		player = GetComponentInParent<Player> ();
		//middle.text = "Which ring would you like to replace?\n";
	}



	// Update is called once per frame
	void Update () {
		left.text = "Left Ring:\n" + player.leftRing.description;
		right.text = "Right Ring:\n" + player.rightRing.description;
	}
}
