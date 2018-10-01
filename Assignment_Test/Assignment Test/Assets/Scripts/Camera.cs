using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

	int speed = 20;
	Player player;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = FindObjectOfType<Player> ();
			transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -5);
		} else {
			transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, -5);
		}
	}
}
