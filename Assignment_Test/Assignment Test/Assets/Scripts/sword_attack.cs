using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword_attack : MonoBehaviour {
	public float speed;
	bool facing_right;
	public Player player;
	public float scaling;
	public GameObject knight;

	// Use this for initialization

	void Start () {
		knight = GameObject.FindGameObjectWithTag("Player");
		facing_right = player.facing_right;
	}
	
	// Update is called once per frame
	void Update () {
		knight = GameObject.FindGameObjectWithTag("Player");
		transform.position = knight.transform.position;


		if (facing_right!=player.facing_right) {
			
			if (player.facing_right) {
				transform.Rotate(0, 0, -1);
				facing_right = true;
			} else {
				transform.Rotate(0, 0, -1);
				facing_right = false;
			}

		} 



		transform.Rotate (Vector3.forward * Time.deltaTime * speed);

	

		if(transform.eulerAngles.z> 135){
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "EnemyArcher") {
			enemyArcher enemy = other.GetComponent<enemyArcher> ();
			enemy.ApplyDamage ((int)(knight.GetComponent<Player> ().damage * scaling));

			Destroy (gameObject);
		} else if (other.tag == "EnemyLight") {
			enemyLight enemy = other.GetComponent<enemyLight> ();
			enemy.ApplyDamage ((int)(knight.GetComponent<Player> ().damage * scaling));

			Destroy (gameObject);
		} else if (other.tag == "EnemyHeavy") {
			enemyHeavy enemy = other.GetComponent<enemyHeavy> ();
			enemy.ApplyDamage ((int)(knight.GetComponent<Player> ().damage * scaling));

			Destroy (gameObject);
		} else if (other.tag == "Boss") {
			Boss enemy = other.GetComponent<Boss> ();
			enemy.ApplyDamage((int)(knight.GetComponent<Player>().damage * scaling));
			Destroy (gameObject);
		}
	}
}
