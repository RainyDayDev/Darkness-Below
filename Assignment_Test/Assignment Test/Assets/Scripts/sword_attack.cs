using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword_attack : MonoBehaviour {
	public float speed;
	bool facing_right;
	public Player player;
	public float scaling;
	public GameObject knight;
    public float time_of_attack;
    CharacterStats myStats;

	// Use this for initialization

	void Start () {
		knight = GameObject.FindGameObjectWithTag("Player");
		facing_right = player.facing_right;
        time_of_attack = 0.14f;
        knight.GetComponent<Player>().lockMovement = true;
        myStats = knight.GetComponent<CharacterStats>();
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
        //transform.Rotate (Vector3.forward * Time.deltaTime * speed);
        if(time_of_attack <= 0){
            knight.GetComponent<Player>().lockMovement = false;
            Destroy (gameObject);
        }else{
            time_of_attack -= Time.deltaTime;
            //knight.GetComponent<Player>().lockMovement = true;
        }
	}

	void OnTriggerEnter2D(Collider2D other){

        if (other.tag == "EnemyArcher") {
            enemyArcher enemy = other.GetComponent<enemyArcher> ();
            enemy.ApplyDamage (myStats.damage.GetValue());
            knight.GetComponent<Player>().lockMovement = false;
            Destroy (gameObject);

        } else if (other.tag == "EnemyLight") {
            //CharacterStats enemy = other.GetComponent<CharacterStats> ();

            //enemy.TakeDamage(myStats.damage.GetValue());

            enemyLight enemy = other.GetComponent<enemyLight>();
            //enemy.ApplyDamage ((int)(knight.GetComponent<Player> ().damage * scaling));
            enemy.ApplyDamage(myStats.damage.GetValue());
            knight.GetComponent<Player>().lockMovement = false;
            Destroy(gameObject);
        }
        else if (other.tag == "EnemyHeavy") {
            enemyHeavy enemy = other.GetComponent<enemyHeavy> ();
            enemy.ApplyDamage(myStats.damage.GetValue());
            knight.GetComponent<Player>().lockMovement = false;
            Destroy(gameObject);

        } else if (other.tag == "Boss") {
            Boss enemy = other.GetComponent<Boss> ();
            enemy.ApplyDamage(myStats.damage.GetValue());
            knight.GetComponent<Player>().lockMovement = false;
            Destroy (gameObject);
        }
                

    }
}
