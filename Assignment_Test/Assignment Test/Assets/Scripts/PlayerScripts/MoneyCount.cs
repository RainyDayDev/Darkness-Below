using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoneyCount : MonoBehaviour {
	Player player;
	public Text text;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = FindObjectOfType<Player> ();
		} else {
			text.text = "Money: " + player.money;
		}

	}
}
