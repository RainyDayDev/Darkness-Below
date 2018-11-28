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
    Vector3 z = new Vector3(0.0f, 0.0f, 1.0f);

    // Use this for initialization

    void Start () {
		knight = GameObject.FindGameObjectWithTag("Player");
        facing_right = knight.GetComponent<Player>().facing_right;
        if (knight.GetComponent<Player>().facing_right)
        {
            transform.Rotate(0, 0, -1);
        }
        time_of_attack = 0.14f;
        knight.GetComponent<Player>().lockMovement = true;
        myStats = knight.GetComponent<CharacterStats>();
	}
	
	// Update is called once per frame
	void Update () {
        knight = GameObject.FindGameObjectWithTag("Player");
        transform.position = knight.transform.position;
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
            enemy.ApplyDamage (myStats.damage.GetValue(), 0);
            knight.GetComponent<Player>().lockMovement = false;
            Destroy (gameObject);

        } else if (other.tag == "EnemyLight") {
            //CharacterStats enemy = other.GetComponent<CharacterStats> ();

            //enemy.TakeDamage(myStats.damage.GetValue());

            enemyLight enemy = other.GetComponent<enemyLight>();
            //enemy.ApplyDamage ((int)(knight.GetComponent<Player> ().damage * scaling));
            if (facing_right)
            {
                enemy.ApplyDamage(myStats.damage.GetValue(), 1);
            }else{
                enemy.ApplyDamage(myStats.damage.GetValue(), 2);
            }
            knight.GetComponent<Player>().lockMovement = false;
            Destroy(gameObject.GetComponent<EdgeCollider2D>());
        }
        else if (other.tag == "EnemyHeavy") {
            enemyHeavy enemy = other.GetComponent<enemyHeavy> ();
            if (facing_right)
            {
                enemy.ApplyDamage(myStats.damage.GetValue(), 1);
            }
            else
            {
                enemy.ApplyDamage(myStats.damage.GetValue(), 2);
            }
            knight.GetComponent<Player>().lockMovement = false;
            Destroy(gameObject.GetComponent<EdgeCollider2D>());

        } /*else if (other.tag == "Boss") {
            Boss enemy = other.GetComponent<Boss> ();
            if (facing_right)
            {
                enemy.ApplyDamage(myStats.damage.GetValue(), 1);
            }
            else
            {
                enemy.ApplyDamage(myStats.damage.GetValue(), 2);
            }
            knight.GetComponent<Player>().lockMovement = false;
            Destroy(gameObject.GetComponent<EdgeCollider2D>());
        }*/
                

    }
}
