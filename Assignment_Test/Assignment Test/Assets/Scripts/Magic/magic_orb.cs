using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic_orb : MonoBehaviour {
	public float magic_speed;
	public float rotation_speed;
    public GameObject explosion;
    CharacterStats myStats;
    GameObject knight;
    float timeOfAttack;
    // Use this for initialization
    void Start () {
        knight = GameObject.FindGameObjectWithTag("Player");
        myStats = knight.GetComponent<CharacterStats>();
        timeOfAttack = 0.14f;
        knight.GetComponent<Player>().lockMovement = true;
    }
	void OnCollisionEnter2D(Collision2D coll) {
		Destroy (gameObject);
	}

	// Update is called once per frame
	void Update () {
        transform.Translate(-Vector3.right * Time.deltaTime * magic_speed);
        //transform.position = knight.transform.position;
        //transform.Rotate (Vector3.forward * Time.deltaTime * speed);
        if (timeOfAttack <= 0)
        {
            knight = GameObject.FindGameObjectWithTag("Player");
            knight.GetComponent<Player>().lockMovement = false;
            //Destroy(gameObject);
        }
        else
        {
            timeOfAttack -= Time.deltaTime;
            //knight.GetComponent<Player>().lockMovement = true;
        }
    }

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "EnemyArcher") {
			enemyArcher enemy = other.GetComponent<enemyArcher> ();
			enemy.ApplyDamage (myStats.magic.GetValue(), 0);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy (gameObject);
		}
		else if (other.tag == "EnemyLight") {
			enemyLight enemy = other.GetComponent<enemyLight> ();
            enemy.ApplyDamage(myStats.magic.GetValue(), 0);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy (gameObject);

		}
		else if (other.tag == "EnemyHeavy") {
			enemyHeavy enemy = other.GetComponent<enemyHeavy> ();
			enemy.ApplyDamage (myStats.magic.GetValue(), 0);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy (gameObject);
		}
	}
}
