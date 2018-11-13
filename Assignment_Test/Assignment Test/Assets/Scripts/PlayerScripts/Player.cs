using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {
    //base control----------------------------------------------------------
    public bool lockMovement =false;
    public float speed;
	public float running_speed;
	public float attack_time_light;
	public float attack_time_heavy;
	public float attack_time_magic;
	public float health = 100;
    public float targetHealth = 100;
	public float baseHealth = 100;
	public float maxHealth = 100;
	public float damage = 15;
	public float baseDamage = 15;
	Vector3 last_position;
	Animator anim;

    //attack prefabs---------------------------------------------------------
    public GameObject light_attack;
	public GameObject heavy_attack;
	public magic_spawn magic_spawn;
	public int level = 1;

    //krises ring confusion--------------------------------------------------
    public Vector3 curDirection;
	public GameObject leftRing;
	public bool leftRingEquipped;
	public GameObject rightRing;
	public bool rightRingEquipped;
	public GameObject ringMenu;
	public bool isRingMenuShowing = false;

    //Fake inventory---------------------------------------------------------
    public int money = 0;
	public int potionCount = 3;
	public int maxPotions = 3;
	public int key = 0;

    //UI---------------------------------------------------------------------
	public Text moneyCount;
	public Slider healthSlider;
	public Text healthText;
	public Text damageText;
	public Text potionText;
	public Button leftButton;
	public Button rightButton;
	public Text middle;
	public Text keyText;
	public GameObject deathText;
	public bool isSpawned = false;
	public bool didDie = false;
	float heavyTimer;
	float magicTimer;

    //sounds------------------------------------------------------------------
    public AudioSource heavy_sound;
	public AudioSource light_sound;
	public AudioSource magic_sound;
	public AudioSource hit_sound;
	public AudioSource death_sound;

    public Material material;
    private float colourValue;


	float attack_count;
	public bool facing_right = false;

    public PlayerStats stats;

	// Use this for initialization

	void Awake(){
		DontDestroyOnLoad (gameObject);
	}

	void Start () {
		attack_count = -1;
		anim = GetComponent<Animator> ();
		last_position = transform.position;
		curDirection = new Vector3 ();
		ringMenu.SetActive (false);
		PlayerPrefs.SetInt ("HighestLevel", level);
		healthSlider.maxValue = maxHealth;
		moneyCount.text = "x " + money;
		potionText.text = "x " + potionCount + "/" + maxPotions;
		keyText.text = "x " + key;
		damageText.text = "" + damage;
		//healthText.text = health + "/" + maxHealth;
        rightRingEquipped = false;
        leftRingEquipped = false;
        stats = GetComponent<PlayerStats>();
        stats.maxHealth = (int)maxHealth;
        stats.currentHealth = (int)maxHealth;

        //material = new Material(Shader.Find("Custom/FlashingRed"));
    }

	//changes variables for animation state machine
	void state_change(int to_change){

		if (to_change == 6 || to_change == 7) {
			anim.SetInteger ("x", to_change);
		}

		if (attack_count < 0) {
			if (to_change == 3) {
				attack_count = attack_time_light;
			}
			if (to_change == 4) {
				attack_count = attack_time_heavy;
			}
			if (to_change == 5) {
				attack_count = attack_time_magic;
			}
			anim.SetInteger ("x", to_change);
		} 
		return;
	}


    public void ApplyDamage(int damage)
    {
        //health -= damage;

        //targetHealth -= damage;
        stats.TakeDamage(damage);
        targetHealth = stats.currentHealth;
        if (damage < 0)
        {
            material.SetColor("_FlashColor", Color.green);
        }
        else
        {
            material.SetColor("_FlashColor", Color.red);
            hit_sound.Play();
        }
        //healthSlider.value = health;
        colourValue = .9f;
        if (stats.currentHealth > (int)maxHealth)
        {
            stats.currentHealth = (int)maxHealth;

        }
        healthText.text = stats.currentHealth + "/" + stats.maxHealth;
    }





    // Update is called once per frame----------------------------------------
    void Update () {
        if (Time.timeScale == 0) {
            return;
        }
		if (ringMenu == null) {
			ringMenu = GameObject.FindGameObjectWithTag ("RingMenu");
		}
		if (SceneManager.GetActiveScene ().name == "Tavern") {
			health = maxHealth;
			healthSlider.value = health;
			healthText.text = health + "/" + maxHealth;
		}
		Vector3 temp = transform.position;

        //Inventory


	//MOVEMENT------------------------------------------------------------
		Vector3 y = new Vector3( 0.0f, 1.0f, 0.0f);
		Vector3 x = new Vector3( 1.0f, 0.0f, 0.0f);
		Vector3 flip = new Vector3(0, 180, -2 * transform.eulerAngles.z);


        /*
Movement to do
-simplify running to one if statement


        */
        if (!lockMovement){
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(y * Time.deltaTime * speed * running_speed);
                state_change(2);
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
                {
                    transform.Translate(y * Time.deltaTime * speed);
                    state_change(1);
                }
            }
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(-y * Time.deltaTime * speed * running_speed);
                state_change(2);
            }
            else
            {
                if (Input.GetKey(KeyCode.S))
                {
                    transform.Translate(-y * Time.deltaTime * speed);
                    state_change(1);
                }
            }
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(-x * Time.deltaTime * speed * running_speed);
                if (facing_right == true)
                {
                    transform.localEulerAngles = transform.eulerAngles + flip;
                    facing_right = false;
                }
                state_change(2);
            }
            else
            {
                if (Input.GetKey(KeyCode.A))
                {
                    transform.Translate(-x * Time.deltaTime * speed);
                    if (facing_right == true)
                    {
                        transform.localEulerAngles = transform.eulerAngles + flip;
                        facing_right = false;
                    }
                    state_change(1);
                }
            }
            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(-x * Time.deltaTime * speed * running_speed);
                if (facing_right == false)
                {
                    transform.localEulerAngles = transform.eulerAngles + flip;
                    facing_right = true;
                }
                state_change(2);
            }
            else
            {
                if (Input.GetKey(KeyCode.D))
                {
                    transform.Translate(-x * Time.deltaTime * speed);
                    if (facing_right == false)
                    {
                        transform.localEulerAngles = transform.eulerAngles + flip;
                        facing_right = true;
                    }
                    state_change(1);
                }
            }
            }

		//IDLE_CHECK------------------------------------------------------------
		if (last_position == transform.position) {
			state_change (0);
		}
		last_position = transform.position;
		//ATTACKING-------------------------------------------------------------
		if (attack_count < 0) {
			//light attack
			if (Input.GetKey (KeyCode.J) || Input.GetMouseButtonDown(0)) {
				Instantiate (light_attack, transform.position, transform.rotation);
				state_change (3);
				light_sound.Play ();
			}
			//heavy attack
			if ((Input.GetKey (KeyCode.K) || Input.GetMouseButtonDown(1)) && heavyTimer <= 0) {
				Instantiate (heavy_attack, transform.position, transform.rotation);
				state_change (4);
				heavy_sound.Play ();
				heavyTimer = 3;
			}
			//magic attack
			if (Input.GetKey (KeyCode.L) && magicTimer <= 0) {
				magic_spawn.fire ();
				state_change (5);
				magic_sound.Play ();
				magicTimer = 5;
			}
		}else {
			attack_count -= Time.deltaTime;
		}
		heavyTimer -= Time.deltaTime;
		magicTimer -= Time.deltaTime;

		//DYING------------------------------------------------------------------
		if (stats.currentHealth <= 0) {
			death_sound.Play ();
			state_change (7);
			didDie = true;
			SceneManager.LoadScene ("Tavern");
			stats.currentHealth = stats.maxHealth;
			level = 1;
			money = 0;
			moneyCount.text = "x " + money;
			healthSlider.value = stats.maxHealth;
            targetHealth = stats.maxHealth;



		}

		//Update curDirection based on starting position and current direction
		curDirection = (transform.position - temp).normalized;
		curDirection.z = 0;



        //Drink potion
		if (Input.GetKeyDown (KeyCode.P)) {
			if (potionCount > 0) {
				ApplyDamage (-20);
				potionCount--;
				potionText.text = "x " + potionCount + "/" + maxPotions;
			}
		}

        //Hit flash
        if(colourValue > 0)
        {
            material.SetFloat("_FlashAmount", colourValue);
            colourValue -= .05f;
        }

        //Healthslider smooth transition
        if(targetHealth > health){
            health = Mathf.Lerp(health, targetHealth, 2*Time.deltaTime);
            healthSlider.value = health;
        }else if(targetHealth < health){
            health = Mathf.Lerp(health, targetHealth, 2 * Time.deltaTime);
            healthSlider.value = health;
        }

    }
}
