using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class enemyArcher : MonoBehaviour {

    public const int Patrolling = 0;
    public const int Attacking = 1;
	public const int Dying = 2;
    public float detectionDistance = 10;
    public float reloadTime = 7;
    public float reloading = 0;
    public float followDistance = 5;
    public int state = Patrolling;
    private float speed = 2;
    public Player player;
    public float health = 30;
    public Bow bow;
	public Text text;
	public MapGenerator mapGenerator;
	public MoneyPickup gem;
	public HealthPickup heart;
	public PotionPickup potion;
	public KeyPickup key;

	Animator archer_anim;
	public float maxHealth;


	private Vector3 wandering;
	private bool tester = true;
	private float testing = 2.0f;
	private float timing;
	private bool facingRight = false;
	private bool facingLeft = true;
	private Vector3 tracker;
	public Material material;

    // Use this for initialization
    void start()
    {
		archer_anim = GetComponent<Animator> ();
		mapGenerator = FindObjectOfType<MapGenerator> ();
    }


	void state_change(int to_change){
		archer_anim.SetInteger ("x", to_change);
	}

    // Update is called once per frame
    void Update () {
		tracker = transform.position;
		player = FindObjectOfType<Player> ();
		if (mapGenerator == null) {
			mapGenerator = FindObjectOfType<MapGenerator> ();
		}else if (player == null) {
			player = FindObjectOfType<Player> ();
		}else if (archer_anim == null) {
			health = health + player.level * 10;
			maxHealth = health;
			text.text = health + "/" + maxHealth;
			archer_anim = GetComponent<Animator> ();
		} else {
			
			switch (state) {
			//Moves around a bit
			case Patrolling:
				state_change (0);
				wander ();
				if ((player.transform.position - transform.position).magnitude < detectionDistance) {
					state = Attacking;
				}
                
				break;
			//Keep distance while firing arrows at the player 
			case Attacking:

				Vector3 target;
				if (timing <= 0) {
					state_change (1);
				} else {
					timing -= Time.deltaTime;
				}
				float bulletCalc = (player.transform.position - transform.position).magnitude / 10 + 1; // the ten is the bullet speed so if that changes we will have to update this
				if ((player.transform.position - transform.position).magnitude > (followDistance + .2f)) {
					moveTo (player.transform);
					state_change (1);

				} else if ((player.transform.position - transform.position).magnitude < (followDistance - 0.2f)) {
					moveAway (player.transform);
					state_change (1);

				} 
				if ((player.transform.position - transform.position).magnitude > 6)
					target = (player.transform.position - transform.position) + player.GetComponent<Player> ().curDirection * (player.transform.position - transform.position).magnitude * bulletCalc;
				else
					target = (player.transform.position - transform.position).normalized;

				if (reloading <= 0) {
					reloading = reloadTime;
					bow.fireAt (target);
				}

				if (reloading <= .5f) {
					state_change (2);
					timing = .5f;
				}
				if(archer_anim.GetInteger("x")==0 ){
					state_change (1);
				}

				reloading -= Time.deltaTime;

				if ((player.transform.position - transform.position).magnitude > detectionDistance * 3 / 2) {// eventually replace this with a raycast to see if the enemy can see them... maybe
					state = Patrolling;
				}
				break;

			case Dying:
				if (timing <= 0) {
					Destroy (gameObject);
				}
				timing -= Time.deltaTime;
				break;
			}
			Vector3 flip = new Vector3 (0, 180, -2 * transform.eulerAngles.z);
			if (transform.position.x - tracker.x >= 0) {
				if (facingLeft) {
					transform.localEulerAngles = transform.eulerAngles + flip;
					text.transform.localEulerAngles = text.transform.localEulerAngles + flip;
					facingLeft = false;
					facingRight = true;
				}
			} else {
				if (facingRight) {
					transform.localEulerAngles = transform.eulerAngles + flip;
					text.transform.localEulerAngles = text.transform.localEulerAngles + flip;
					facingLeft = true;
					facingRight = false;
				}
			}
		}
    }

    //Move towards the transform given
    public void moveTo(Transform place)
    {
        Vector3 temp = (place.position - transform.position).normalized;
		temp.z = 0;
        gameObject.transform.position += temp * speed * Time.deltaTime;
    }

    //Move away from the transform given
    public void moveAway(Transform place)
    {
        Vector3 temp = (place.position - transform.position).normalized;
		temp.z = 0;
        gameObject.transform.position -= temp * speed * Time.deltaTime;
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
		material.SetColor("_FlashColor", Color.red);
		text.text = health + "/"+maxHealth;
        if(health <= 0){
			state_change (3);
			Drop();
			timing = .7f;
			state = Dying;
			text.text = "";
        }
    }

	public void wander()
	{
		Vector3 temp;
		if(tester)
		{
			temp = ((Vector3)Random.insideUnitCircle);
			temp.z = 0;
			wandering = temp;
			tester = false;
		}
		else
		{
			temp = wandering;
		}
		testing -= Time.deltaTime;
		if(testing <= 0)
		{
			tester = true;
			testing = 2.0f;
		}
		gameObject.transform.position += temp * speed * Time.deltaTime;
	}


	public void Drop()
	{
		float range = Random.Range(0, 100);
		if (range >= 95)
		{
			//Instantiate potion
			Instantiate(potion, transform.position, transform.rotation);
		}
		else if (range >= 60 && range < 95)
		{
			Instantiate(heart, transform.position, transform.rotation);
			//Instantiate heart
		}
		if (range >= 98) {
			Instantiate (key, transform.position, transform.rotation);
		} else {
			//Instantiate money
			int money = Random.Range (player.GetComponent<Player>().level, player.GetComponent<Player> ().level * 5);
			gem.value = money;
			Instantiate (gem, transform.position, transform.rotation);
		}
	}
}
    