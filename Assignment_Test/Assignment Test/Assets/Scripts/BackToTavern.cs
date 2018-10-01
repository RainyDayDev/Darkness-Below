using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BackToTavern : MonoBehaviour {
	bool isTrigger = false;
	Player player;
	int cost;
	public GameObject canvas;
	public Text text;
	MapGenerator mapGenerator;
	// Use this for initialization
	void Start () {
		mapGenerator = FindObjectOfType<MapGenerator> ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Player")){
			isTrigger = true;	
			player = other.GetComponent<Player> ();
			if (player.level % 5 == 0) {
				cost = 0;
			} else {
				cost = (player.level - 1) * 10;
			}
			text.text = "This staircase will let you head back to the Tavern for " + cost + " gems. Press E to accept";
			canvas.SetActive (true);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.CompareTag("Player")){
			isTrigger = false;
			canvas.SetActive (false);
		}
	}

	// Update is called once per frame
	void Update () {
		if (isTrigger) {
			if (Input.GetKeyDown (KeyCode.E) && player.money >= cost && mapGenerator.bossSpawned == false) {
				player.money -= cost;
				player.moneyCount.text = "x " + player.money;
				SceneManager.LoadScene ("Tavern");
			}
		}
	}
}
