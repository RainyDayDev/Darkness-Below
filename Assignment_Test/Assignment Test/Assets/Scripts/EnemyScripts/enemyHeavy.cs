using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyHeavy : MonoBehaviour {

    public const int Patrolling = 0;
    public const int Attacking = 1;
    public float knockback;
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
    private float timer = 4.5f;
	private float timing = 1f;
    Animator light_anim;
    public float maxHealth;
	private bool facingRight = false;
	private bool facingLeft = true;
	private Vector3 tracker;
    public GameObject successfulAttack;

	public Material material;
	private float colourValue;

    public EnemyStats stats;

    private float attackDistance = 1f;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
    }

    // Use this for initialization
    void Start()
    {
		player = FindObjectOfType<Player> ();
		light_anim = GetComponent<Animator>();
		mapGenerator = FindObjectOfType<MapGenerator>();
		int random = Random.Range (4, 8);
		health = health + random * player.currentLevel;
        stats.maxHealth = (int)health;
        stats.currentHealth = stats.maxHealth;

        text.text = stats.currentHealth + "/" + stats.maxHealth;
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
			colourValue -= .05f;
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
           // int random = Random.Range(4, 8);
            //health = health + random * player.level;
            //maxHealth = health;
            //text.text = health + "/" + maxHealth;
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

                    moveTo(player.transform);
                    if (timing <= 0)
                    {
                        state_change(1);
                    }
                    else
                    {
                        timing -= Time.deltaTime;
                    }
                    //state_change(1);

                    if ((player.transform.position - transform.position).magnitude < 1 && timer <= 0)
                    {
                        state_change(2);
                        GameObject go = Instantiate(attack, transform.position, transform.rotation) as GameObject;
                        go.SendMessage("TheStart", facingRight);


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
            Vector3 flip = new Vector3(0, 180, -2 * transform.eulerAngles.z);
            if (transform.position.x - tracker.x > 0)
            {
                if (facingLeft)
                {
                    transform.localEulerAngles = transform.eulerAngles + flip;
                    text.transform.localEulerAngles = text.transform.localEulerAngles + flip;
                    facingLeft = false;
                    facingRight = true;
                }
            }
            else if (transform.position.x - tracker.x < 0)
            {
                if (facingRight)
                {
                    transform.localEulerAngles = transform.eulerAngles + flip;
                    text.transform.localEulerAngles = text.transform.localEulerAngles + flip;
                    facingLeft = true;
                    facingRight = false;
                }
            }
        }
	}


    public void moveTo(Transform place)
    {
        Vector3 direction = (place.position - transform.position).normalized;

        direction.z = 0;
        Vector3 distance = place.position - transform.position;

        if (Mathf.Abs(distance.x) >= attackDistance || Mathf.Abs(distance.y) >= attackDistance)
        {
            gameObject.transform.position += direction * speed * Time.deltaTime;
        }
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
	public void ApplyDamage(float damage, int isRight)
	{
        stats.TakeDamage((int)damage);
        text.text = stats.currentHealth + "/" + stats.maxHealth;
        GetComponent<Renderer>().material.SetColor("_FlashColor", Color.red);
        Instantiate(successfulAttack, transform.position, transform.rotation);
        colourValue = .9f;
        if (isRight == 1)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector3(knockback, 0f, 0f);
        }
        else if (isRight == 2)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector3(-knockback, 0f, 0f);
        }


        if (stats.currentHealth <= 0)
        {
            state_change(3);
            Drop();
            timing = .7f;
            state = Dying;
            text.text = "";
        }
    }


	public void Drop()
	{
		float range = Random.Range(0, 100);
		if (range >= 99) {
			Instantiate (key, transform.position, transform.rotation);
		}
		else if (range >= 90)
		{
			//Instantiate potion
			Instantiate(potion, transform.position, transform.rotation);
		}
		else if (range >= 70 && range < 90)
		{
			Instantiate(heart, transform.position, transform.rotation);
			//Instantiate heart
		}
		else if(range >= 40 && range < 70) {
			//Instantiate money
			int money = Random.Range (player.GetComponent<Player>().currentLevel, player.GetComponent<Player> ().currentLevel * 5);
			gem.value = money;
			Instantiate (gem, transform.position, transform.rotation);
		}
	}
}