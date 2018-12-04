using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
	bool isTrigger = false;
    public ItemPickup item;
	Player player;
	// Use this for initialization
	MapGenerator mapGenerator;
	void Start () {
		mapGenerator = FindObjectOfType<MapGenerator> ();
		player = FindObjectOfType<Player> ();
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			isTrigger = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			isTrigger = true;
		}
	}
	// Update is called once per frame
	void Update () {
		if (isTrigger) {
			if(Input.GetKeyDown(KeyCode.E) && player.key > 0){
				player.key--;
				player.keyText.text = "x " + player.key;
                player.currentLevel += 2;
				Instantiate (item, transform.position, transform.rotation);
                player.currentLevel -= 2;
				Destroy (gameObject);
			}
		}
	}
}
