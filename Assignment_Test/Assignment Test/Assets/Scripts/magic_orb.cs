using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic_orb : MonoBehaviour {
	bool facing_right;
	public float magic_speed;
	public float rotation_speed;
	// Use this for initialization
	void Start () {
	}
	void OnCollisionEnter2D(Collision2D coll) {
		Destroy (gameObject);
	}

	// Update is called once per frame
	void Update () {

			transform.Translate (-Vector3.right * Time.deltaTime * magic_speed);

			
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "EnemyArcher") {
			enemyArcher enemy = other.GetComponent<enemyArcher> ();
			enemy.ApplyDamage (15);
			Destroy (gameObject);
		}
		else if (other.tag == "EnemyLight") {
			enemyLight enemy = other.GetComponent<enemyLight> ();
			enemy.ApplyDamage (15);

			Destroy (gameObject);
		}
		else if (other.tag == "EnemyHeavy") {
			enemyHeavy enemy = other.GetComponent<enemyHeavy> ();
			enemy.ApplyDamage (15);

			Destroy (gameObject);
		}else if (other.tag == "Boss") {
			Boss enemy = other.GetComponent<Boss> ();
			enemy.ApplyDamage(15);
			Destroy (gameObject);
		}
	}
}
