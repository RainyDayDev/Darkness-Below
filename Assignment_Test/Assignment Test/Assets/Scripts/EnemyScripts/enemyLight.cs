using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyLight : MonoBehaviour {
    public const int Patrolling = 0;
    public const int Attacking = 1;
	public const int Dying = 2;
    public float detectionDistance = 10;
    public int state = Patrolling;
    private float speed = 1;
    public Player player;
    public float health = 30;
    public Text text;
    public MapGenerator mapGenerator;
    public MoneyPickup gem;
    public HealthPickup heart;
	public PotionPickup potion;
	public KeyPickup key;

    private Vector3 wandering;
    private bool tester = true;
    private float testing = 2.0f;
    public GameObject attack;
    //public int damage = 20;
    private float timer = 3;
	private float timing = 1;
    Animator light_anim;
    public float maxHealth;
	private bool facingRight = false;
	private bool facingLeft = true;
	private Vector3 tracker;
	public Material material;
	private float colourValue;

    // Use this for initialization
    void start()
    {
		player = FindObjectOfType<Player> ();
        light_anim = GetComponent<Animator>();
        mapGenerator = FindObjectOfType<MapGenerator>();
		health = health + 10 * player.level;
		maxHealth = health;
		GetComponent<Renderer>().material.CopyPropertiesFromMaterial(material);
    }

    void state_change(int to_change)
    {
        light_anim.SetInteger("x", to_change);
    }
    // Update is called once per frame
    void Update()
    {
		if(colourValue > 0)
		{
			GetComponent<Renderer>().material.SetFloat("_FlashAmount", colourValue);
			this.colourValue -= .05f;
		}
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
        else if (light_anim == null)
        {
            health = health + player.level * 10;
            maxHealth = health;
            text.text = health + "/" + maxHealth;
            light_anim = GetComponent<Animator>();
        }
        else
        {
            switch (state)
            {
                //Moves around a bit
                case Patrolling:

                    state_change(1);
                    wander();
                    if ((player.transform.position - transform.position).magnitude < detectionDistance)
                    {
                        state = Attacking;
                    }

                    break;
                //Moves in towards player and attacks 
			case Attacking:

				moveTo (player.transform);
				if (timing <= 0) {
					state_change (1);
				} else {
					timing -= Time.deltaTime;
				}
                //state_change(1);

                    if ((player.transform.position - transform.position).magnitude < 1 && timer <= 0)
                    {
                        state_change(2);
                        Instantiate(attack, transform.position, transform.rotation);
                        timer = 3;
						timing = 1f;
                    }
                    timer -= Time.deltaTime;
                    if ((player.transform.position - transform.position).magnitude > detectionDistance * 3 / 2)// eventually replace this with a raycast to see if the enemy can see them... maybe
                    {
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

    //Move towards the transform given, will eventually have to impliment A*
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
		text.text = health + "/" + maxHealth;
		GetComponent<Renderer>().material.SetColor("_FlashColor", Color.red);
		colourValue = .9f;


        if (health <= 0)
        {
			state_change (3);
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
			Instantiate(potion, transform.position, transform.rotation);
		}
		else if (range >= 50 && range < 95)
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
