using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_test : MonoBehaviour {

	Rigidbody2D rigidbody;
	Vector3 velocity;
	public int level = 0;
	public int health = 100;
	public HealthBar healthBar;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		velocity = new Vector3 (Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);	
	}

	void FixedUpdate(){
		rigidbody.MovePosition ((Vector2)rigidbody.position + (Vector2)velocity * 10*  Time.fixedDeltaTime);
	}

	public void ApplyDamage(){
		health -= 10;
		healthBar.SetHealth (health);
		if (health <= 0) {
			//Destroy (gameObject);
		}

	}
}
