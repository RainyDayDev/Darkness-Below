using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour {

    public const int Patrolling = 0;
    public const int Attacking = 1;
    public const int Dying = 2;
    public float detectionDistance = 100;
    public int state = Patrolling;
    private float speed = 1;
    public Player player;
    public float health = 150;
    public Text text;
    public MapGenerator mapGenerator;
    public MoneyPickup gem;
    public HealthPickup heart;
	public Slider healthSlider;
    private Vector3 wandering;
    private bool tester = true;
    private float testing = 2.0f;
    public GameObject attack;
    //public int damage = 20;
    private float timer = 4.5f;
    private float timing = 1f;
    Animator boss_anim;
    public float maxHealth;
	private bool facingRight = false;
	private bool facingLeft = true;
	private Vector3 tracker;

    // Use this for initialization
    void start()
    {
        player = FindObjectOfType<Player>();
        boss_anim = GetComponent<Animator>();
        mapGenerator = FindObjectOfType<MapGenerator>();
       // health = health + 100 * player.level;
        maxHealth = health;
		//text.text = health+"/" + maxHealth;
		healthSlider.maxValue = maxHealth;
		//healthSlider.minValue = 0;
    }

    void state_change(int to_change)
    {
        boss_anim.SetInteger("x", to_change);
    }
    // Update is called once per frame
    void Update()
    {
		tracker = transform.position;
        player = FindObjectOfType<Player>();
        if (mapGenerator == null)
        {
            mapGenerator = FindObjectOfType<MapGenerator>();
        }
        else if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        else if (boss_anim == null)
        {
            health = health + player.level * 10;
            maxHealth = health;
			//text.text = health+"/" + maxHealth;
			healthSlider.maxValue = maxHealth;
			healthSlider.value = maxHealth;
            boss_anim = GetComponent<Animator>();
        }
        else
        {
            switch (state)
            {
                //Moves around a bit
                case Patrolling:

                    //state_change(1);
                    wander();
                    if ((player.transform.position - transform.position).magnitude < detectionDistance)
                    {
                        state = Attacking;
                    }

                    break;
                //Moves in towards player and attacks 
                case Attacking:

                    moveTo(player.transform);
                    if (timing <= 0)
                    {
                        //state_change(1);
                    }
                    else
                    {
                        timing -= Time.deltaTime;
                    }
                    //state_change(1);

                    if ((player.transform.position - transform.position).magnitude < 6 && timer <= 0)
                    {
                        state_change(2);
                        Instantiate(attack, transform.position, transform.rotation);
                        timer = 4.5f;
                        timing = 1f;
                    }
                    timer -= Time.deltaTime;
                    if ((player.transform.position - transform.position).magnitude > detectionDistance * 3 / 2)// eventually replace this with a raycast to see if the enemy can see them... maybe
                    {
                        state = Patrolling;
                    }
                    break;
                case Dying:
                    if (timing <= 0)
                    {
                        Destroy(gameObject);
                    }
                    timing -= Time.deltaTime;
                    break;
            }
			Vector3 flip = new Vector3 (0, 180, -2 * transform.eulerAngles.z);
			if (transform.position.x - tracker.x >= 0) {
				if (facingLeft) {
					transform.localEulerAngles = transform.eulerAngles + flip;
					healthSlider.transform.localEulerAngles = transform.localEulerAngles + flip;
					//text.transform.localEulerAngles = text.transform.eulerAngles + flip;
					facingLeft = false;
					facingRight = true;
				}
			} else {
				if (facingRight) {
					transform.localEulerAngles = transform.eulerAngles + flip;
					healthSlider.transform.localEulerAngles = transform.localEulerAngles + flip;
					//text.transform.localEulerAngles = text.transform.eulerAngles + flip;
					facingLeft = true;
					facingRight = false;
				}
			}
        }
    }


    public void moveTo(Transform place)
    {
        Vector3 temp = (place.position - transform.position).normalized;
        temp.z = 0;
        gameObject.transform.position += temp * speed * Time.deltaTime;
    }

    //Choose a random direction and wanders for 2 seconds
    public void wander()
    {
        Vector3 temp;
        if (tester)
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
        if (testing <= 0)
        {
            tester = true;
            testing = 2.0f;
        }
        gameObject.transform.position += temp * speed * Time.deltaTime;
    }

    //Applies damage to player and sets the shader colour
    public void ApplyDamage(float damage)
    {
        health -= damage;
        //text.text = health + "/" + maxHealth;
		healthSlider.value = health;
        if (health <= 0)
        {
            state_change(3);
            Drop();
            timing = .7f;
            state = Dying;
            text.text = "";
            //Destroy(gameObject);
        }
    }


    public void Drop()
    {
        float range = Random.Range(0, 100);
        if (range >= 95)
        {
            //Instantiate potion
        }
        else if (range >= 60 && range < 95)
        {
            Instantiate(heart, transform.position, transform.rotation);
            //Instantiate heart
        }

        //Instantiate money
		int money = Random.Range(player.GetComponent<Player>().level * 10, player.GetComponent<Player>().level * 15);
        gem.value = money;
        Instantiate(gem, transform.position, transform.rotation);
    }

	void OnDestroy(){
		//Vector2 spawn = mapGenerator.findSpawn (mapGenerator.);
		//Instantiate (mapGenerator.stairs, spawn, transform.rotation);
		//spawn = mapGenerator.findBossSpawn ();
		//Instantiate (mapGenerator.exitStairs, spawn, transform.rotation);
		mapGenerator.bossSpawned = false;
	}
}
