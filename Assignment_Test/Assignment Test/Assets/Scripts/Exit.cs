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
			player.currentLevel++;
			if (player.currentLevel > PlayerPrefs.GetInt ("HighestLevel")) {
				PlayerPrefs.SetInt ("HighestLevel", player.currentLevel);
			}
            if (player.currentLevel > player.farthestLevel) {
                player.farthestLevel = player.currentLevel;
            }
			mapGenerator.width = mapGenerator.width + player.currentLevel * 2;
			mapGenerator.height = mapGenerator.height + player.currentLevel * 2;
			mapGenerator.randomFillPercent = 50;
			mapGenerator.GenerateMap ();
		}
	}
		
}
