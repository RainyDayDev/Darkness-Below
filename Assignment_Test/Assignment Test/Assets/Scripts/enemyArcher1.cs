using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class enemyArcher1 : MonoBehaviour {

    public const int Patrolling = 0;
    public const int Attacking = 1;
    public const int AlmostDead = 2;
    public const int Healin = 3;
    public float detectionDistance = 10;
    public float reloadTime = 7;
    public float reloading = 0;
    public float followDistance = 5;
    public int state = Patrolling;
    private float speed = 2;
    private Transform startPoint;
    public GameObject player;
    public float health = 100;
    public float healthTreshold;
    public int healRate = 1;
    public static float healthCap;
    public Bow bow;
	public Text text;
	public MapGenerator mapGenerator;
	public MoneyPickup gem;
	public HealthPickup heart;
	Animator archer_anim;
	public Material material;
    // Use this for initialization
    void start()
    {
		archer_anim = GetComponent<Animator> ();
		mapGenerator = FindObjectOfType<MapGenerator> ();
        startPoint = transform;
        player = GameObject.FindWithTag("Player");
        //health = health * level number
        healthTreshold = health * 2 / 10;
        healthCap = health;
        //healRate = some equation to scale healing rate
    }

	void state_change(int to_change){
		archer_anim.SetInteger ("x", to_change);
	}


    // Update is called once per frame
    void Update () {
		if (mapGenerator == null) {
			mapGenerator = FindObjectOfType<MapGenerator> ();
		}
        player = GameObject.FindWithTag("Player");
		if (archer_anim == null) {
			archer_anim = GetComponent<Animator> ();
		} else {
			switch (state) {
			//Moves around a bit
			case Patrolling:
				state_change (0);
				if ((player.transform.position - transform.position).magnitude < detectionDistance) {
					state = Attacking;
				}
                
				break;
			//Keep distance while firing arrows at the player 
			case Attacking:
				Vector3 target;
				float bulletCalc = (player.transform.position - transform.position).magnitude / 10 + 1; // the ten is the bullet speed so if that changes we will have to update this
				if ((player.transform.position - transform.position).magnitude > followDistance) {
					moveTo (player.transform);
					if(reloading<=0){
						state_change (1);
					}
				} else if ((player.transform.position - transform.position).magnitude < followDistance) {
					moveAway (player.transform);
					if(reloading<=0){
						state_change (1);
					}
				} else {
					if(reloading<=0){
						state_change (0);
					}
				}
				

				target = (player.transform.position - transform.position) + player.GetComponent<Player> ().curDirection * (player.transform.position - transform.position).magnitude * bulletCalc;
                //target = (player.transform.position - transform.position).normalized;

				if (reloading <= 0) {
					reloading = reloadTime;
					bow.fireAt (target);//This will be replaced by target
					state_change (2);

				}

				reloading -= Time.deltaTime;

				if ((player.transform.position - transform.position).magnitude > detectionDistance * 3 / 2) {// eventually replace this with a raycast to see if the enemy can see them... maybe
					state = Patrolling;
				}

				if (health < healthTreshold) {
					state = AlmostDead;
				}
				break;
			// Move to start point then heal 
			case AlmostDead:
//                moveTo(startPoint);
//                if (transform.position == startPoint.position)
  //              {
    //                state = Healin;
      //          }
				break;
			//If at the start point heal up
			case Healin:
				health += healRate;
				if (health >= healthCap) {
					state = Patrolling;
				}
				break;
			}
		}
    }

    //Move towards the transform given, will eventually have to impliment A*
    public void moveTo(Transform place)
    {
        Vector3 temp = (place.position - transform.position).normalized;
		temp.z = 0;
        gameObject.transform.position += temp * speed * Time.deltaTime;
    }

    //Move away from the transform given, wil eventually have to impliment A*
    public void moveAway(Transform place)
    {
        Vector3 temp = (place.position - transform.position).normalized;
		temp.z = 0;
        gameObject.transform.position -= temp * speed * Time.deltaTime;
    }

    public void ApplyDamage(float damage)
    {
        if(state != AlmostDead)
        {
            state = Attacking;
        }
		material.SetColor("_FlashColor", Color.red);
        health -= damage;
		text.text = health + "/100";
        if(health <= 0){
			Drop ();
            Destroy(gameObject);
        }
    }


	public void Drop(){
		float range = Random.Range (0, 100);
		if (range >= 95) {
			//Instantiate potion
		}
		else if (range >= 60 && range < 95) {
			Instantiate (heart, transform.position, transform.rotation);
			//Instantiate heart
		}

		//Instantiate money
		int money = Random.Range (1, player.GetComponent<Player> ().level * 5);
		gem.value = money;
		Instantiate (gem, transform.position, transform.rotation);
	}
}
    