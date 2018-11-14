using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic_spawn : MonoBehaviour {
	public GameObject magic_attack;
	// Use this for initialization
	void Start () {
		
	}
	public void fire(){
		Instantiate (magic_attack, transform.position, transform.rotation);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
