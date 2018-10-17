using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Ring : MonoBehaviour {

	public int healthBonus;
	public int damageBonus;
	bool inTrigger = false;
	Player player;
	public string description;
	bool statsSet = false;
	public bool isTavern = false;
	public int value = 0;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player> ();
		if (SceneManager.GetActiveScene ().name == "Tavern") {
			isTavern = true;
		}
		if(player != null && isTavern == true){
			int random = Random.Range (1, 4);
			healthBonus = 10 + PlayerPrefs.GetInt("HighestLevel") * random;
			random = Random.Range (1, 4);
			damageBonus = 10 + PlayerPrefs.GetInt("HighestLevel") * random;
			description = "Health Bonus = " + healthBonus + "\nDamage Bonus = " + damageBonus;
			statsSet = true;
			value = healthBonus + damageBonus * 5;
		}
		else if (player != null && isTavern == false) {
			int random = Random.Range (1, 4);
			healthBonus = 10 + player.level * random;
			random = Random.Range (1, 4);
			damageBonus = 10 + player.level * random;
			description = "Health Bonus = " + healthBonus + "\nDamage Bonus = " + damageBonus;
			statsSet = true;
			value = healthBonus + damageBonus * 5;
		}
	}
	public void setRing(Ring newRing){
		healthBonus = newRing.healthBonus;
		damageBonus = newRing.damageBonus;
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Player")){
			inTrigger = true;
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if(other.CompareTag("Player")){
			inTrigger = false;
		}
	}

	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = FindObjectOfType<Player> ();
		} /*else if (statsSet == false) {
			int random = Random.Range (1, 4);
			healthBonus = 10 + player.level * random;
			damageBonus = 10 + player.level * random;
			description = "Health Bonus = " + healthBonus + "\nDamage Bonus = " + damageBonus;
			statsSet = true;
		}*/ else {
			if (inTrigger) {
				if (Input.GetKeyDown (KeyCode.E) && isTavern == false) {
					if (player.rightRingEquipped == false && player.leftRingEquipped == true) {
						player.rightRing.setRing (this);
						player.rightRingEquipped = true;
						player.damage = player.damage + this.damageBonus;
						player.maxHealth = player.maxHealth + this.healthBonus;
						player.rightRing.description = this.description;
						player.healthText.text = player.health + "/" + player.maxHealth;
						player.damageText.text = "" + player.damage;
						player.healthSlider.maxValue = player.maxHealth;
						Destroy (gameObject);
					} else if (player.leftRingEquipped == false && player.rightRingEquipped == false) {
						player.leftRing.setRing (this);
						player.leftRingEquipped = true;
						player.damage = player.damage + this.damageBonus;
						player.maxHealth = player.maxHealth + this.healthBonus;
						player.healthSlider.maxValue = player.maxHealth;
						player.leftRing.description = this.description;
						player.damageText.text = "" + player.damage;
						player.healthText.text = player.health + "/" + player.maxHealth;
						Destroy (gameObject);
					} else {
						int newDamage = this.damageBonus;
						int newHealth = this.healthBonus;
						string newDescription = this.description;
						player.middle.text = "Which ring would you like to replace?\n" + this.description;
						player.leftButton.onClick.AddListener (delegate {
							player.LeftRingDamage (newDamage);
						});
						player.leftButton.onClick.AddListener (delegate {
							player.LeftRingHealth (newHealth);
						});
						player.leftButton.onClick.AddListener (delegate {
							player.LeftRingDescription (newDescription);
						});
						player.rightButton.onClick.AddListener (delegate {
							player.RightRingDamage (newDamage);
						});
						player.rightButton.onClick.AddListener (delegate {
							player.RightRingHealth (newHealth);
						});
						player.rightButton.onClick.AddListener (delegate {
							player.RightRingDescription (newDescription);
						});
						Time.timeScale = 0;
						player.ringMenu.SetActive (true);

					} 
					Destroy (gameObject);
				}

				if (Input.GetKeyDown (KeyCode.E) && isTavern == true && player.money >= this.value) {
					if (player.rightRingEquipped == false && player.leftRingEquipped == true) {
						player.rightRing.setRing (this);
						player.rightRingEquipped = true;
						player.damage = player.damage + this.damageBonus;
						player.maxHealth = player.maxHealth + this.healthBonus;
						player.rightRing.description = this.description;
						player.healthSlider.maxValue = player.maxHealth;
						player.damageText.text = "" + player.damage;
						player.healthText.text = player.health + "/" + player.maxHealth;
						Destroy (gameObject);
					} else if (player.leftRingEquipped == false && player.rightRingEquipped == false) {
						player.leftRing.setRing (this);
						player.leftRingEquipped = true;
						player.damage = player.damage + this.damageBonus;
						player.maxHealth = player.maxHealth + this.healthBonus;
						player.healthSlider.maxValue = player.maxHealth;
						player.leftRing.description = this.description;
						player.damageText.text = "" + player.damage;
						player.healthText.text = player.health + "/" + player.maxHealth;
						Destroy (gameObject);
					} else {
						int newDamage = this.damageBonus;
						int newHealth = this.healthBonus;
						string newDescription = this.description;
						player.middle.text = "Which ring would you like to replace?\n" + this.description;
						player.leftButton.onClick.AddListener (delegate {
							player.LeftRingDamage (newDamage);
						});
						player.leftButton.onClick.AddListener (delegate {
							player.LeftRingHealth (newHealth);
						});
						player.leftButton.onClick.AddListener (delegate {
							player.LeftRingDescription (newDescription);
						});
						player.rightButton.onClick.AddListener (delegate {
							player.RightRingDamage (newDamage);
						});
						player.rightButton.onClick.AddListener (delegate {
							player.RightRingHealth (newHealth);
						});
						player.rightButton.onClick.AddListener (delegate {
							player.RightRingDescription (newDescription);
						});
						Time.timeScale = 0;
						player.ringMenu.SetActive (true);

					} 
					player.money -= this.value;
					player.moneyCount.text = "x " + player.money;
					Destroy (gameObject);
				}
			}
		}
	}
}
