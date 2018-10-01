using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernScript : MonoBehaviour {
	public Player player;
	public Player spawnedPlayer;
	public GameObject deathText;
	// Use this for initialization
	void Start () {
		spawnedPlayer = FindObjectOfType<Player> ();
		if (!spawnedPlayer) {
			spawnedPlayer = Instantiate (player, transform.position, transform.rotation);
		} else {
			spawnedPlayer.transform.position = gameObject.transform.position;
		}
		if (spawnedPlayer.didDie) {
			Instantiate (deathText, transform.position, transform.rotation);
			spawnedPlayer.didDie = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
