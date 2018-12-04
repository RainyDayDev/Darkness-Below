using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Sellable : MonoBehaviour {
	public int value;
	bool isTrigger = false;
	Player player;
	Ring ring;
	public GameObject canvas;
	//public Text text;
	public Text toolTip;
	bool isRing = false;
	bool isKey = false;
	bool isPotionBelt = false;
	bool isPotion = false;
	string Description;
	// Use this for initialization
	void Start () {

		if (this.gameObject.CompareTag ("Ring")) {
			ring = GetComponent<Ring> ();
			if (ring != null) {
				value = ring.value;
			}
			Description = "This ring will boost your health and damage while in the dungeon.\n"+ring.description;
			toolTip.text = Description;
			isRing = true;
		} else if (this.gameObject.CompareTag ("Key")) {
			value = 100;
			Description = "This key will enable you to unlock rare treasure chests found in the dungeon.\nCost = "+value;
			toolTip.text = Description;
			isKey = true;
		} else if (this.gameObject.CompareTag ("PotionBelt")) {
			value = 100;
			Description = "This potion belt allows you to carry one additional potion with you. Comes with a potion just because we're nice!\nCost = "+value;
			toolTip.text = Description;
			isPotionBelt = true;
		} else if (this.gameObject.CompareTag ("Potion")) {
			value = 30;
			Description = "This potion will heal you during your adventure. But you can only carry so many!\nCost = "+value;
			toolTip.text = Description;
			isPotion = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Player")){
			isTrigger = true;
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
		if (player == null) {
			player = FindObjectOfType<Player> ();
		} else {
			if (isTrigger) {
				if (Input.GetKeyDown (KeyCode.E) && isKey && player.money >= value) {
					player.key++;
					player.keyText.text = "x " + player.key;
					player.money -= value;
					player.moneyCount.text = "x " + player.money;
				} else if (Input.GetKeyDown (KeyCode.E) && isPotion && player.money >= value && player.potionCount < player.maxPotions) {
					player.potionCount++;
					player.potionText.text = "x " +player.potionCount + "/" + player.maxPotions;
					player.money -= value;
					player.moneyCount.text = "x " + player.money;
				} else if (Input.GetKeyDown (KeyCode.E) && isPotionBelt && player.money >= value) {
					player.maxPotions++;
					player.potionCount++;
					player.potionText.text = "x "+ player.potionCount + "/" + player.maxPotions;
					player.money -= value;
					player.moneyCount.text = "x " + player.money;
				}
			}
		}
	}
}
