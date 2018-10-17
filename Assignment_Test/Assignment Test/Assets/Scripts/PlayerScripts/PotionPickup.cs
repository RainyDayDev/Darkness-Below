using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPickup : MonoBehaviour {
	Vector3 startPos;
	float time;
	// Use this for initialization
	void Start () {
		Player player = FindObjectOfType<Player> ();
		startPos = transform.position;
		Destroy (gameObject, 30);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			if (other.GetComponent<Player> ().potionCount < other.GetComponent<Player> ().maxPotions) {
				other.GetComponent<Player> ().potionCount++;
				other.GetComponent<Player> ().potionText.text = "x " + other.GetComponent<Player> ().potionCount + "/" + other.GetComponent<Player>().maxPotions;
				Destroy (gameObject);
			} 
		}
	}
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		Vector3 y = new Vector3 (0.0f, 1.0f, 0.0f);
		Vector3 x = new Vector3 (1.0f, 0.0f, 0.0f);
		if (time <= 0.1f) {
			transform.Translate (y * Time.deltaTime * 4);
			transform.Translate (x * Time.deltaTime * 4);
		}
		if (time > 0.1f && transform.position.y > startPos.y) {
			transform.Translate (-y * Time.deltaTime * 4);
		}
	}
}
