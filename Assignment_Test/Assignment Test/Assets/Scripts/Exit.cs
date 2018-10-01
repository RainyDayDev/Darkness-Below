using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {
	public MapGenerator mapGenerator;
	Player player;

	void Start(){
		player = FindObjectOfType<Player> ();
		mapGenerator = FindObjectOfType<MapGenerator> ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.CompareTag("Player") && mapGenerator.bossSpawned == false){
			player.level++;
			if (player.level > PlayerPrefs.GetInt ("HighestLevel")) {
				PlayerPrefs.SetInt ("HighestLevel", player.level);
			}
			mapGenerator.width = mapGenerator.width + player.level * 2;
			mapGenerator.height = mapGenerator.height + player.level * 2;
			mapGenerator.randomFillPercent = 50;
			mapGenerator.GenerateMap ();
		}
	}
		
}
