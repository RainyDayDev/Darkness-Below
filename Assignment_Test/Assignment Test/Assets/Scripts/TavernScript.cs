using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernScript : MonoBehaviour {
	public Player player;
	public Player spawnedPlayer;
    public InventoryUI inventory;
	public GameObject deathText;
    public GameObject gameManager;
	// Use this for initialization
	void Start () {
		spawnedPlayer = FindObjectOfType<Player> ();
		if (!spawnedPlayer) {
            Instantiate(gameManager, transform.position, transform.rotation);
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
